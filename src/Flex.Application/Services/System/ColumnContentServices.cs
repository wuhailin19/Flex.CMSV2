using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices.System;
using Flex.Core.Config;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Core.Framework.Enum;
using Flex.Dapper;
using Flex.Domain.Cache;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Enums.LogLevel;
using Flex.Domain.WhiteFileds;
using Flex.SqlSugarFactory.Seed;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Collections;
using System.Data;
using System.Text;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices : BaseService, IColumnContentServices
    {
        protected MyDBContext _dapperDBContext;
        protected MyContext _sqlsugar;
        protected IRoleServices _roleServices;
        protected IWorkFlowServices _workFlowServices;
        protected ICaching _caching;
        protected ISystemLogServices _logServices;
        private readonly IServiceProvider _serviceProvider;

        ISqlTableServices _sqlTableServices;
        public ColumnContentServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims
            , MyDBContext dapperDBContext, ISqlTableServices sqlTableServices, IRoleServices roleServices, ICaching caching
            , IWorkFlowServices workFlowServices, MyContext sqlsugar, ISystemLogServices logServices
            , IServiceProvider serviceProvider)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _dapperDBContext = dapperDBContext;
            _sqlTableServices = sqlTableServices;
            _roleServices = roleServices;
            _caching = caching;
            _workFlowServices = workFlowServices;
            _sqlsugar = sqlsugar;
            _logServices = logServices;
            _serviceProvider = serviceProvider;
        }
        private void InitAddTimeAndPublishTime(Hashtable table)
        {
            var addtime = table.GetValue("AddTime")?.ToString() ?? string.Empty;
            var publishtime = table.GetValue("PublishTime")?.ToString() ?? string.Empty;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    if (addtime.IsNullOrEmpty())
                        table["AddTime"] = localTime;
                    else
                        table["AddTime"] = addtime.ToUtcTime();
                    if (publishtime.IsNullOrEmpty())
                        table["PublishTime"] = localTime;
                    else
                        table["PublishTime"] = publishtime.ToUtcTime();
                    break;
                default:
                    if (addtime.IsNullOrEmpty())
                        table["AddTime"] = Clock.Now;
                    if (publishtime.IsNullOrEmpty())
                        table["PublishTime"] = Clock.Now;
                    break;
            }
        }
        private void InitCreateTable(Hashtable table)
        {
            table["AddUser"] = _claims.UserId;
            table["AddUserName"] = _claims.UserName;
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    table["LastEditDate"] = localTime;
                    break;
                default:
                    table["LastEditDate"] = Clock.Now;
                    break;
            }
            InitAddTimeAndPublishTime(table);
            table["OrderId"] = 0;
        }
        private void InitUpdateTable(Hashtable table)
        {
            //如果是审批结束
            if (table.GetValue("StatusCode").ToInt() != 5)
            {
                table["ReviewStepId"] = string.Empty;
                table["ReviewAddUser"] = 0;
                table["MsgGroupId"] = 0;
            }
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    table["LastEditDate"] = localTime;
                    break;
                default:
                    table["LastEditDate"] = Clock.Now;
                    break;
            }
            InitAddTimeAndPublishTime(table);
        }
        private void InitDeleteTable(Hashtable table)
        {
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                    table["LastEditDate"] = localTime;
                    break;
                default:
                    table["LastEditDate"] = Clock.Now;
                    break;
            }
        }
        

        public async Task<ProblemDetails<int>> Add(Hashtable table, bool IsReview = false)
        {
            int parentId = table["ParentId"].ToInt();
            int ModelId = table["ModelId"].ToInt();
            if (!await CheckPermission(parentId, nameof(DataPermissionDto.ad)))
                return Problem(HttpStatusCode.BadRequest, 0, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == parentId);
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return Problem<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == ModelId);
            if (contentmodel == null)
                return Problem(HttpStatusCode.BadRequest, 0, ErrorCodes.DataInsertError.GetEnumDescription());
            table.SetValue("ParentId", parentId);
            table.SetValue("SiteId", CurrentSiteInfo.SiteId);

            var filedmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == ModelId)).ToList();
            var keysToRemove = new List<object>();
            var timelist = new List<object>();
            var white_fileds = ColumnContentUpdateFiledConfig.defaultFields.ToList();

            foreach (var item in table.Keys)
            {
                if (white_fileds.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (table[item].ToString().IsTime())
                {
                    timelist.Add(item);
                }
                if (!filedmodel.Any(m => m.FieldName.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }

            //pgSQL情况
            switch (DataBaseConfig.dataBase)
            {
                case DataBaseType.PgSql:
                    foreach (var item in timelist)
                    {
                        table[item] = table[item].ToString().ToUtcTime();
                    }
                    break;
                default:
                    break;
            }
            foreach (var key in keysToRemove)
            {
                table.Remove(key);
            }
            InitCreateTable(table);

            //生成内容组Id
            table.SetValue("ContentGroupId", _idWorker.NextId());
            string orderSql = _sqlTableServices.GetNextOrderIdDapperSqlString(contentmodel.TableName);

            var orderId = _sqlsugar.Db.Ado.GetDataTable(orderSql)?.Rows[0][0].ToInt() ?? 0;

            //DynamicParameters parameters = new DynamicParameters();
            //StringBuilder builder = _sqlTableServices.CreateDapperInsertSqlString(table, contentmodel.TableName, orderId, out parameters);

            SugarParameter[] parameters = [];
            StringBuilder builder = _sqlTableServices.CreateSqlsugarInsertSqlString(table, contentmodel.TableName, orderId, out parameters);
            try
            {
                //var result = _dapperDBContext.ExecuteScalar(builder.ToString(), parameters);
                var result = _sqlsugar.Db.Ado.GetScalar(builder.ToString(), parameters).ToInt();
                if (result > 0)
                {
                    if (table.ContainsKey("Title"))
                    {
                        await _logServices.AddContentLog(SystemLogLevel.Normal, $"新增数据【{table.GetValue("Title")}】到栏目【{column.Name}】", $"普通新增");
                    }
                    return Problem(HttpStatusCode.OK, result, ErrorCodes.DataInsertSuccess.GetEnumDescription());
                }
                return Problem(HttpStatusCode.BadRequest, 0, ErrorCodes.DataInsertError.GetEnumDescription());
            }
            catch (Exception ex)
            {
                return Problem<int>(HttpStatusCode.InternalServerError, ErrorCodes.DataInsertError.GetEnumDescription(), ex);
            }
        }
        private async Task<bool> CheckPermission(int ParentId, string permissioncate)
        {
            if (_claims.IsSystem)
                return true;
            Dictionary<int, Dictionary<string, List<string>>> datapermissionList;
            var cachekey = RoleKeys.userDataPermissionKey + _claims.UserRole;

            if (_caching.Get(cachekey) == null)
            {
                var currentrole = await _roleServices.GetCurrentRoldDtoAsync();
                if (currentrole == null)
                    return false;
                var datapermission = JsonHelper.Json<List<sitePermissionDto>>(currentrole.DataPermission ?? string.Empty);
                if (datapermission == null)
                    return false;
                var currentsitepermission = datapermission.Where(m => m.siteId == CurrentSiteInfo.SiteId).FirstOrDefault();
                if (currentsitepermission == null)
                    return false;
                datapermissionList = new Dictionary<int, Dictionary<string, List<string>>>();
                Dictionary<string, List<string>> filedvalue = new Dictionary<string, List<string>>
                {
                    { nameof(DataPermissionDto.sp), currentsitepermission.columnPermission.sp.ToList("-") },
                    { nameof(DataPermissionDto.ed), currentsitepermission.columnPermission.ed.ToList("-") },
                    { nameof(DataPermissionDto.ad), currentsitepermission.columnPermission.ad.ToList("-") },
                    { nameof(DataPermissionDto.dp), currentsitepermission.columnPermission.dp.ToList("-") }
                };
                datapermissionList.Add(_claims.UserRole, filedvalue);
                _caching.Set(cachekey, datapermissionList, new TimeSpan(1, 0, 0));
            }
            else
            {
                datapermissionList = _caching.Get(cachekey) as Dictionary<int, Dictionary<string, List<string>>> ?? new Dictionary<int, Dictionary<string, List<string>>>();
            }
            if (datapermissionList == null)
                return false;
            if (!datapermissionList.ContainsKey(_claims.UserRole))
                return false;
            return datapermissionList[_claims.UserRole][permissioncate].Contains(ParentId.ToString());
        }
        public async Task<ProblemDetails<string>> Delete(int ParentId, int modelId, string Id)
        {
            if (!await CheckPermission(ParentId, nameof(DataPermissionDto.dp)))
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == modelId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            string Ids = Id.Replace("-", ",");
            try
            {
                SqlSugar.SugarParameter[] parameters;
                Hashtable hashtable = new Hashtable();
                InitDeleteTable(hashtable);
                hashtable.SetValue("Ids", Ids);
                hashtable.SetValue("StatusCode", "0");
                var deletestr = _sqlTableServices.CreateSqlsugarUpdateSqlString(hashtable, contentmodel.TableName, out parameters).ToString();
                _sqlsugar.Db.Ado.ExecuteCommand(deletestr, parameters);
                await _logServices.AddContentLog(SystemLogLevel.Warning, $"删除栏目【{column.Name}】{Ids.Split(',').Count()}条数据，Id为【{Ids}】", $"删除");
                return Problem<string>(HttpStatusCode.OK, $"共删除{Ids.Split(',').Count()}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }
        public async Task<ProblemDetails<string>> RestContent(int ParentId, int modelId, string Id)
        {
            if (!await CheckPermission(ParentId, nameof(DataPermissionDto.ed)))
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == modelId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataRestError.GetEnumDescription());
            string Ids = Id.Replace("-", ",");
            try
            {
                _unitOfWork.ExecuteSqlCommand(_sqlTableServices.RestContentTableData(contentmodel.TableName, Ids));
                await _unitOfWork.SaveChangesAsync();
                await _logServices.AddContentLog(SystemLogLevel.Warning, $"恢复栏目【{column.Name}】{Ids.Split(',').Count()}条数据，Id为【{Ids}】", $"回收站恢复");
                return Problem<string>(HttpStatusCode.OK, $"共恢复{Ids.Split(',').Count()}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataRestError.GetEnumDescription(), ex);
            }
        }
        /// <summary>
        /// 完全删除
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<string>> CompletelyDelete(int ParentId, int modelId, string Id)
        {
            if (!await CheckPermission(ParentId, nameof(DataPermissionDto.dp)))
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == modelId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            string Ids = Id.Replace("-", ",");
            try
            {
                _unitOfWork.ExecuteSqlCommand(_sqlTableServices.CompletelyDeleteContentTableData(contentmodel.TableName, Ids));
                await _unitOfWork.SaveChangesAsync();
                await _logServices.AddContentLog(SystemLogLevel.Warning, $"完全删除栏目【{column.Name}】{Ids.Split(',').Count()}条数据，Id为【{Ids}】", $"删除");
                return Problem<string>(HttpStatusCode.OK, $"共完全删除{Ids.Split(',').Count()}条数据");
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.DataDeleteError.GetEnumDescription(), ex);
            }
        }
    }
}
