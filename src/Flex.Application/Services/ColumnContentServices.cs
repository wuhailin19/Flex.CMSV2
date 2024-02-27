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
using System.IO;
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
            ",Title,Id,AddTime,StatusCode,ReviewAddUser,AddUserName,LastEditUserName,OrderId,ParentId,ReviewStepId,ContentGroupId,MsgGroupId,";


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
            var result = await _dapperDBContext.GetDynamicAsync("select " + filed + " from " + contentmodel.TableName + " where ParentId=" + ParentId + " and StatusCode=1");
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
        public async Task<Page> ListAsync(ContentPageListParamDto contentPageListParam)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == contentPageListParam.ParentId);
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
            DynamicParameters parameters = new DynamicParameters();
            string swhere = string.Empty;
            if (contentPageListParam.k.IsNotNullOrEmpty())
            {
                parameters.Add("@k", contentPageListParam.k);
                if (contentPageListParam.k.ToInt() != 0)
                    swhere += " and (Title like '%'+@k+'%' or Id=cast(@k as int))";
                else
                    swhere += " and Title like '%'+@k+'%'";
            }
            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            {
                parameters.Add("@timefrom", contentPageListParam.timefrom);
                swhere += " and AddTime>=@timefrom";
            }
            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            {
                parameters.Add("@timeto", contentPageListParam.timeto);
                swhere += " and AddTime>=@timeto";
            }
            var result = await _dapperDBContext.PageAsync(contentPageListParam.page, contentPageListParam.limit, "select " + filed + " from " + contentmodel.TableName + " where StatusCode not in (0,6)" + swhere, parameters);
            result.Items.Each(item =>
            {
                item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                item.StatusCode = ((StatusCode)item.StatusCode).GetEnumDescription();
            });
            return result;
        }

        public async Task<OutputContentAndWorkFlowDto> GetButtonListByParentId(int ParentId)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.ad)))
                return default;
            var stepActionButtonDto = new List<StepActionButtonDto> { };
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            if (column.ReviewMode.ToInt() != 0)
            {
                stepActionButtonDto = (await _workFlowServices.GetStepActionButtonList(new InputWorkFlowStepDto { flowId = column.ReviewMode.ToInt(), stepPathId = string.Empty })).ToList();
            }
            var model = new OutputContentAndWorkFlowDto
            {
                Content = null,
                stepActionButtonDto = stepActionButtonDto,
                NeedReview = column.ReviewMode.ToInt() != 0
            };
            return model;
        }
        public async Task<Dictionary<object, object>> GetContentForReviewById(int ParentId, int Id)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return default;
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return default;
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId)).ToList();
            string filed = "ReviewAddUser,MsgGroupId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@ParentId", ParentId);
            var result = (await _dapperDBContext.GetDynamicAsync("select " + filed + " from " + contentmodel.TableName + " where ParentId=@ParentId and Id=@Id", parameters)).FirstOrDefault();
            Dictionary<object, object> normalItems = new Dictionary<object, object>();
            if (result == null)
                return normalItems;
            foreach (KeyValuePair<string, object> col in result)
            {
                if (!normalItems.ContainsKey(col.Key))
                    normalItems.Add(col.Key, col.Value);
            }
            return normalItems;
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
                stepActionButtonDto = new List<StepActionButtonDto> { },
                NeedReview = column.ReviewMode.ToInt() != 0,
                OwnerShip = result.ReviewAddUser == _claims.UserId || _claims.IsSystem
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
        public async Task<SysContentModel> GetSysContentModelByColumnId(int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            return contentmodel;
        }
        public async Task<ProblemDetails<int>> Add(Hashtable table, bool IsReview = false)
        {
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ad)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, 0, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await GetSysContentModelByColumnId(column.Id);
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

        /// <summary>
        /// 修改数据内容
        /// </summary>
        /// <param name="table">前端传入字段</param>
        /// <param name="IsReview">当前修改模式是否为审核</param>
        /// <param name="white_fileds">本次修改的字段白名单</param>
        /// <param name="IsCancelReview">取消审批确认参数</param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> Update(
            Hashtable table
            , bool IsReview = false
            , List<string> white_fileds = null
            , bool IsCancelReview = false)
        {
            if (table.Count == 0)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());

            var filedmodel = await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == column.ModelId);
            ClearNotUseFields(table, white_fileds, filedmodel);

            InitUpdateTable(table);
            //当前表中字段用于生成修改副本
            var fileds = defaultFields.ToList();
            foreach (var item in filedmodel)
            {
                fileds.Add(item.FieldName);
            }
            return await UpdateContentCore(table, contentmodel, fileds);
        }

        private static void ClearNotUseFields(Hashtable table, List<string> white_fileds, IList<sysField> filedmodel)
        {
            var keysToRemove = new List<object>();
            if (white_fileds.IsNullOrEmpty())
                white_fileds = defaultFields.ToList();
            foreach (var item in table.Keys)
            {
                if (white_fileds.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (!filedmodel.Any(m => m.FieldName.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);
        }

        /// <summary>
        /// 审核后修改的内容
        /// </summary>
        /// <param name="table"></param>
        /// <param name="white_fileds"></param>
        /// <param name="IsCancelReview"></param>
        /// <returns></returns>
        public async Task<ProblemDetails<int>> UpdateReviewContent(
            Hashtable table,
            bool IsReview = true,
            bool IsCancelReview = false)
        {
            var whitefields = new List<string> { "ParentId", "Id", "StatusCode", "ReviewStepId", "ReviewAddUser", "MsgGroupId" };
            if (table.Count == 0)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
            if (!await CheckPermission(table["ParentId"].ToInt(), nameof(DataPermissionDto.ed)))
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == table["ParentId"].ToInt());
            //栏目需审核则不允许正常修改
            if (column.ReviewMode.ToInt() != 0 && !IsReview)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNeedReview.GetEnumDescription());
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
            if (IsCancelReview)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", table["Id"]);
                parameters.Add("@ParentId", table["ParentId"]);
                var result = (await _dapperDBContext.GetDynamicAsync("select ReviewAddUser from " + contentmodel.TableName + " where ParentId=@ParentId and Id=@Id", parameters)).FirstOrDefault();
                if (result == null)
                    return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataNotFound.GetEnumDescription());
                if (result.ReviewAddUser != _claims.UserId && _claims.IsSystem)
                {
                    return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.NoOperationPermission.GetEnumDescription());
                }
            }
            var keysToRemove = new List<object>();

            foreach (var item in table.Keys)
            {
                if (!whitefields.Any(m => m.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase)))
                    keysToRemove.Add(item);
            }
            foreach (var key in keysToRemove)
                table.Remove(key);
            return await UpdateContentCore(table, contentmodel, null);
        }

        private async Task<ProblemDetails<int>> UpdateContentCore(Hashtable table, SysContentModel contentmodel, List<string> fileds)
        {
            StringBuilder builder = new StringBuilder();
            SqlParameter[] commandParameters = new SqlParameter[] { };
            builder = _sqlTableServices.CreateUpdateSqlString(table, contentmodel.TableName, out commandParameters);
            try
            {
                var result = _unitOfWork.ExecuteSqlCommand(builder.ToString(), commandParameters);
                if (fileds != null)
                {
                    //创建历史副本
                    var createcopysql = _sqlTableServices.CreateInsertCopyContentSqlString(fileds, contentmodel.TableName, table["Id"].ToInt());
                    _unitOfWork.ExecuteSqlCommand(createcopysql.ToString());
                }
                if (result > 0)
                {
                    await _unitOfWork.SaveChangesAsync();

                    return new ProblemDetails<int>(HttpStatusCode.OK, ErrorCodes.DataUpdateSuccess.GetEnumDescription());
                }
                return new ProblemDetails<int>(HttpStatusCode.BadRequest, ErrorCodes.DataUpdateError.GetEnumDescription());
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
