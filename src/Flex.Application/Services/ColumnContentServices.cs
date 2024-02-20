using Consul;
using Dapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Contracts.IServices;
using Flex.Dapper;
using Flex.Domain;
using Flex.Domain.Cache;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.WorkFlow;
using Flex.Domain.HtmlHelper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using ShardingCore.Extensions;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Flex.Application.Services
{
    public class ColumnContentServices : BaseService, IColumnContentServices
    {
        MyDBContext _dapperDBContext;
        IRoleServices _roleServices;
        IWorkFlowServices _workFlowServices;

        private ICaching _caching;
        //默认加载字段
        private const string defaultFields = "IsTop,IsRecommend,IsHot,IsShow,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,StatusCode,AddUserName,LastEditUserName,OrderId,ParentId,ReviewStepId,";
        ISqlTableServices _sqlTableServices;
        public ColumnContentServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, MyDBContext dapperDBContext, ISqlTableServices sqlTableServices, IRoleServices roleServices, ICaching caching, IWorkFlowServices workFlowServices)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _dapperDBContext = dapperDBContext;
            _sqlTableServices = sqlTableServices;
            _roleServices = roleServices;
            _caching = caching;
            _workFlowServices = workFlowServices;
        }
        public async Task<ColumnPermissionAndTableHeadDto> GetTableThs(int ParentId)
        {
            var tableths = ModelTools<ColumnContentDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId && m.ShowInTable == true)).ToList();
            foreach (var item in fieldmodel)
            {
                tableths.Add(new ModelTools<ColumnContentDto>()
                {
                    title = item.Name,
                    sort = false,
                    align = "center",
                    maxWidth = "200",
                    field = item.FieldName
                });
            }
            ColumnPermissionAndTableHeadDto columnPermission = new ColumnPermissionAndTableHeadDto();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
        public async Task<IEnumerable<ContentOptions>> GetContentOptions(int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = "Title,Id";
            var options = new List<ContentOptions>();
            var result = await _dapperDBContext.GetDynamicAsync("select " + filed + " from " + contentmodel.TableName + " where ParentId=" + ParentId + " and StatusCode<>0");
            result.Each(item =>
            {
                options.Add(new ContentOptions()
                {
                    text = item.Title,
                    value = item.Id.ToString(),
                    @checked = false
                });
            });
            return options;
        }
        public async Task<Page> ListAsync(int pageindex, int pagesize, int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new Page();
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = defaultFields;
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
            }
            filed = filed.TrimEnd(',');
            var result = await _dapperDBContext.PageAsync(pageindex, pagesize, "select " + filed + " from " + contentmodel.TableName + " where StatusCode<>0");
            result.Items.Each(item =>
            {
                item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                item.StatusCode = ((StatusCode)item.StatusCode).GetEnumDescription();
            });
            return result;
        }

        public async Task<IEnumerable<StepActionButtonDto>> GetButtonListByParentId(int ParentId)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return default;
            var stepActionButtonDto = new List<StepActionButtonDto> { };
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            if (column.ReviewMode.ToInt() != 0)
            {
                stepActionButtonDto = (await _workFlowServices.GetStepActionButtonList(new InputWorkFlowStepDto { flowId = column.ReviewMode.ToInt(), stepPathId = string.Empty })).ToList();
            }
            return stepActionButtonDto;
        }

        public async Task<OutputContentAndWorkFlowDto> GetContentById(int ParentId, int Id)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return default;
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return default;
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = defaultFields;
            //List<string> editors = new List<string>();
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
                //if (item.FieldType == nameof(Editor))
                //    editors.Add(item.FieldName);
            }
            filed = filed.TrimEnd(',');
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@ParentId", ParentId);
            parameters.Add("@flowId", column.ReviewMode);

            var result = (await _dapperDBContext.GetDynamicAsync("select " + filed + ",flowId=@flowId from " + contentmodel.TableName + " where ParentId=@ParentId and Id=@Id", parameters)).FirstOrDefault();

            var model = new OutputContentAndWorkFlowDto
            {
                Content = result,
                stepActionButtonDto = new List<StepActionButtonDto> { }
            };
            if (column.ReviewMode.ToInt() != 0)
            {
                model.stepActionButtonDto = await _workFlowServices.GetStepActionButtonList(new InputWorkFlowStepDto { flowId = column.ReviewMode.ToInt(), stepPathId = result.ReviewStepId });
            }

            #region 废弃
            //UpdateContentDto updateContentDto = new UpdateContentDto();
            //if (result.Count() != 1)
            //    return string.Empty;
            //var currentmodel = result.FirstOrDefault();
            //var filedlist = filed.ToList();
            //Dictionary<object, object> normalItems = new Dictionary<object, object>();
            //Dictionary<object, object> editorItems = new Dictionary<object, object>();

            //foreach (KeyValuePair<string, object> col in currentmodel)
            //{
            //    if (!editors.Contains(col.Key))
            //        normalItems.Add(col.Key, col.Value);
            //    else
            //        editorItems.Add(col.Key, col.Value);
            //}

            //updateContentDto.normalItem = normalItems;
            //updateContentDto.editorItem = editorItems;
            #endregion

            return model;
        }

        public async Task<ProblemDetails<string>> GetFormHtml(int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "没有选择有效模型");
            return new ProblemDetails<string>(HttpStatusCode.OK, contentmodel.FormHtmlString);
        }
        private void InitTable(Hashtable table)
        {
            table["AddUser"] = _claims.UserId;
            table["AddUserName"] = _claims.UserName;
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            table["LastEditDate"] = Clock.Now;
            table["OrderId"] = 0;
        }

        public async Task<ProblemDetails<string>> Add(Hashtable table)
        {
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ad)))
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            var filedmodel = await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId);
            var keysToRemove = new List<object>();
            var white_fileds = defaultFields.ToList();
            foreach (var item in table.Keys)
            {
                if (white_fileds.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (!filedmodel.Any(m => m.FieldName.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
            {
                table.Remove(key);
            }
            InitTable(table);
            StringBuilder builder = new StringBuilder();
            SqlParameter[] commandParameters = new SqlParameter[] { };
            builder = _sqlTableServices.CreateInsertSqlString(table, contentmodel.TableName, out commandParameters);
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);

                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.GetEnumDescription());
                }
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.GetEnumDescription());
            }
            catch
            {
                throw;
            }
        }
        public async Task<ProblemDetails<string>> Update(Hashtable table)
        {
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            var filedmodel = await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId);
            var keysToRemove = new List<object>();
            var white_fileds = defaultFields.ToList();
            foreach (var item in table.Keys)
            {
                if (white_fileds.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (!filedmodel.Any(m => m.FieldName.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
            {
                table.Remove(key);
            }
            InitTable(table);
            StringBuilder builder = new StringBuilder();
            SqlParameter[] commandParameters = new SqlParameter[] { };
            builder = _sqlTableServices.CreateUpdateSqlString(table, contentmodel.TableName, out commandParameters);
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);
                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
                }
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            }
            catch
            {
                throw;
            }
        }
        private async Task<bool> CheckPermission(int ParentId, string filedName)
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
                var datapermission = JsonHelper.Json<DataPermissionDto>(currentrole.DataPermission ?? string.Empty);
                if (datapermission == null)
                    return false;
                datapermissionList = new Dictionary<int, Dictionary<string, List<string>>>();
                Dictionary<string, List<string>> filedvalue = new Dictionary<string, List<string>>
                {
                    { nameof(DataPermissionDto.sp), datapermission.sp.ToList("-") },
                    { nameof(DataPermissionDto.ed), datapermission.ed.ToList("-") },
                    { nameof(DataPermissionDto.ad), datapermission.ad.ToList("-") },
                    { nameof(DataPermissionDto.dp), datapermission.dp.ToList("-") }
                };
                datapermissionList.Add(_claims.UserRole, filedvalue);
                _caching.Set(cachekey, datapermissionList, new TimeSpan(1, 0, 0));
            }
            else
            {
                datapermissionList = _caching.Get(cachekey) as Dictionary<int, Dictionary<string, List<string>>>;
            }
            if (datapermissionList == null)
                return false;
            if (!datapermissionList.ContainsKey(_claims.UserRole))
                return false;
            return datapermissionList[_claims.UserRole][filedName].Contains(ParentId.ToString());
        }
        public async Task<ProblemDetails<string>> Delete(int ParentId, string Id)
        {
            if (!await CheckPermission(ParentId, nameof(DataPermissionDto.dp)))
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataDeleteError.GetEnumDescription());
            string Ids = Id.Replace("-", ",");
            try
            {
                _unitOfWork.ExecuteSqlCommand(_sqlTableServices.DeleteContentTableData(contentmodel.TableName, Ids));
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, $"共删除{Ids.Split(',').Count()}条数据");
            }
            catch
            {
                throw;
            }
        }
    }
}
