using Dapper;
using Flex.Application.Contracts.Exceptions;
using Flex.Domain;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices : BaseService, IColumnContentServices
    {
        public async Task<ColumnPermissionAndTableHeadDto<ColumnContentDto>> GetTableThs(int ParentId)
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
            ColumnPermissionAndTableHeadDto<ColumnContentDto> columnPermission = new ColumnPermissionAndTableHeadDto<ColumnContentDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }
        public async Task<ColumnPermissionAndTableHeadDto<HistoryColumnDto>> GetHistoryTableThs(int ParentId)
        {
            var tableths = ModelTools<HistoryColumnDto>.getColumnDescList();
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            ColumnPermissionAndTableHeadDto<HistoryColumnDto> columnPermission = new ColumnPermissionAndTableHeadDto<HistoryColumnDto>();
            columnPermission.TableHeads = tableths;
            columnPermission.IsDelete = await CheckPermission(ParentId, nameof(DataPermissionDto.dp));
            columnPermission.IsUpdate = await CheckPermission(ParentId, nameof(DataPermissionDto.ed));
            columnPermission.IsAdd = await CheckPermission(ParentId, nameof(DataPermissionDto.ad));
            columnPermission.IsSelect = await CheckPermission(ParentId, nameof(DataPermissionDto.sp));
            return columnPermission;
        }

        public async Task<SysContentModel> GetSysContentModelByColumnId(int ParentId)
        {
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            return contentmodel;
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
        public async Task<Page> HistoryListAsync(ContentPageListParamDto contentPageListParam)
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
            parameters.Add("@parentId", contentPageListParam.ParentId);
            swhere = " and ParentId=@parentId";
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            {
                parameters.Add("@ContentGroupId", contentPageListParam.ContentGroupId);
                swhere += " and ContentGroupId=@ContentGroupId";
            }
            var result = await _dapperDBContext.PageAsync(contentPageListParam.page, contentPageListParam.limit, "select " + filed + " from " + contentmodel.TableName + " where StatusCode=6 order by AddTime desc" + swhere, parameters);
            result.Items.Each(item =>
            {
                item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                item.StatusCode = ((StatusCode)item.StatusCode).GetEnumDescription();
            });
            return result;
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
            parameters.Add("@parentId", contentPageListParam.ParentId);
            swhere = " and ParentId=@parentId";
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
                swhere += " and AddTime<DATEADD(day, 1, @timeto)";
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
        public async Task<ProblemDetails<OutputContentAndWorkFlowDto>> GetContentById(int ParentId, int Id)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.NotFound, ErrorCodes.DataNotFound.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == column.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.NotFound, ErrorCodes.DataNotFound.GetEnumDescription());
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
            if (result == null)
                return new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.NotFound, ErrorCodes.DataNotFound.GetEnumDescription());
            var model = new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.OK)
            {
                Content =
                new OutputContentAndWorkFlowDto
                {
                    Content = result,
                    stepActionButtonDto = new List<StepActionButtonDto> { },
                    NeedReview = column.ReviewMode.ToInt() != 0,
                    OwnerShip = result.ReviewAddUser == _claims.UserId || _claims.IsSystem
                }
            };
            if (column.ReviewMode.ToInt() != 0)
            {
                model.Content.stepActionButtonDto = await _workFlowServices.GetStepActionButtonList(new InputWorkFlowStepDto { flowId = column.ReviewMode.ToInt(), stepPathId = result.ReviewStepId });
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
    }
}
