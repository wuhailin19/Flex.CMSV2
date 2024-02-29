using Castle.Core.Internal;
using Dapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Dapper;
using Flex.Domain;
using Flex.Domain.Cache;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.WorkFlow;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Text;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices : BaseService, IColumnContentServices
    {
        protected MyDBContext _dapperDBContext;
        protected IRoleServices _roleServices;
        protected IWorkFlowServices _workFlowServices;

        protected ICaching _caching;
        //默认加载字段
        protected const string defaultFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,LastEditDate,StatusCode,ReviewAddUser,AddUserName,LastEditUserName,OrderId,ParentId,ReviewStepId,ContentGroupId,MsgGroupId,";

        //修改历史版本字段
        protected const string updatehistoryFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,AddUser,AddUserName,ParentId,ContentGroupId,";

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
        private void InitCreateTable(Hashtable table)
        {
            table["AddUser"] = _claims.UserId;
            table["AddUserName"] = _claims.UserName;
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            table["LastEditDate"] = Clock.Now;
            table["OrderId"] = 0;
        }
        private void InitUpdateTable(Hashtable table)
        {
            table["LastEditUser"] = _claims.UserId;
            table["LastEditUserName"] = _claims.UserName;
            table["LastEditDate"] = Clock.Now;
        }
        public async Task<ProblemDetails<int>> Add(Hashtable table, bool IsReview = false)
        {
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ad)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.DataUpdateError.GetEnumDescription());
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
            InitCreateTable(table);

            //生成内容组Id
            table["ContentGroupId"] = _idWorker.NextId();

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder builder = _sqlTableServices.CreateDapperInsertSqlString(table, contentmodel.TableName, out parameters);
            try
            {
                var result = _dapperDBContext.ExecuteScalar(builder.ToString(), parameters);
                if (result > 0)
                {
                    return new ProblemDetails<int>(HttpStatusCode.OK, result, ErrorCodes.DataInsertSuccess.GetEnumDescription());
                }
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.DataInsertError.GetEnumDescription());
            }
            catch
            {
                throw;
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
            return datapermissionList[_claims.UserRole][permissioncate].Contains(ParentId.ToString());
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
