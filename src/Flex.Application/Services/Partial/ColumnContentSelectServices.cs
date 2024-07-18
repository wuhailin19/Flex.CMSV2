using Dapper;
using Dm;
using Flex.Application.ContentModel;
using Flex.Application.Contracts.Exceptions;
using Flex.Core.Config;
using Flex.Core.Framework.Enum;
using Flex.Dapper;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Dtos.System.TableRelation;
using Flex.Domain.Dtos.WorkFlow;
using Flex.Domain.WhiteFileds;
using Flex.SqlSugarFactory.Seed;
using Microsoft.OpenApi.Expressions;
using System.Data;
using Flex.Domain.Dtos.System.ContentModel;
using Flex.Domain.Dtos.System.ColumnContent;
using OfficeOpenXml.DataValidation;

namespace Flex.Application.Services
{
    public partial class ColumnContentServices
    {
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
            var result = await _dapperDBContext.GetDynamicAsync("select " + filed.GetCurrentBaseField() + " from " + contentmodel.TableName
                + " where ParentId=" + ParentId + " and StatusCode=1");
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
        /// <summary>
        /// 历史版本
        /// </summary>
        /// <param name="contentPageListParam"></param>
        /// <returns></returns>
        public async Task<Page> HistoryListAsync(ContentPageListParamDto contentPageListParam)
        {
            return await AbstractList(contentPageListParam, 3, " order by LastEditDate desc");
        }
        /// <summary>
        /// 回收站
        /// </summary>
        /// <param name="contentPageListParam"></param>
        /// <returns></returns>
        public async Task<Page> SoftDeleteListAsync(ContentPageListParamDto contentPageListParam)
        {
            return await AbstractList(contentPageListParam, 2, " order by LastEditDate desc");
        }
        public async Task<ProblemDetails<ContentExportExcelDto>> GetExportExcelDataTableAsync(ContentPageListParamDto contentPageListParam)
        {
            string StatusCodeExpression = "StatusCode not in (0,6)";
            var resultmodel = new ContentExportExcelDto();
            //resultmodel.UserId = _claims.UserId;

            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == contentPageListParam.ModelId);
            if (contentmodel == null)
                return new ProblemDetails<ContentExportExcelDto>(HttpStatusCode.BadRequest, resultmodel, ErrorCodes.DataNotFound.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == contentPageListParam.ParentId);
            resultmodel.ExcelName = column.Name ?? contentmodel.Name;
            if (contentPageListParam.PId != 0)
            {
                StatusCodeExpression += $" and PId={contentPageListParam.PId}";
                resultmodel.ExcelName = column.Name + "_" + contentmodel.Name;
            }

            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == contentPageListParam.ModelId)).ToList();
            string filed = ColumnContentUpdateFiledConfig.defaultFields;
            resultmodel.filedModels = ContentModelHelper.defaultfileds.ToList();
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
                resultmodel.filedModels.Add(new FiledModel
                {
                    FiledDesc = item.Name,
                    FiledMode = item.FieldType,
                    FiledName = item.FieldName,
                    RealFiledName = item.FieldName
                });
            }
            filed = filed.TrimEnd(',');
            SqlSugar.SugarParameter[] parameters = [];
            string swhere = string.Empty;
            _sqlTableServices.CreateSqlSugarColumnContentSelectSql(contentPageListParam, out swhere, out parameters);
            int recount = 0;
            var result =  _sqlsugar.GetPageDataList(
                contentPageListParam.page,
                contentPageListParam.limit,
                "select " + filed.GetCurrentBaseField()
                + " from " + contentmodel.TableName
                + " where " + StatusCodeExpression + swhere + " order by OrderId desc",ref recount, parameters);

            if(result.Rows.Count==0)
                return new ProblemDetails<ContentExportExcelDto>(HttpStatusCode.BadRequest, resultmodel, ErrorCodes.DataNotFound.GetEnumDescription());
            resultmodel.result = result;

            return new ProblemDetails<ContentExportExcelDto>(HttpStatusCode.OK, resultmodel);
        }
        /// <summary>
        /// 抽象出来的列表函数
        /// </summary>
        /// <param name="contentPageListParam">参数实体</param>
        /// <param name="modetype">区分普通列表/历史列表/软删除列表</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        private async Task<Page> AbstractList(ContentPageListParamDto contentPageListParam, int modetype, string orderby)
        {
            string StatusCodeExpression = string.Empty;
            switch (modetype)
            {
                case 1: StatusCodeExpression = "StatusCode not in (0,6)"; break;
                case 2: StatusCodeExpression = "StatusCode=0"; break;
                case 3: StatusCodeExpression = "StatusCode=6"; break;
            }

            if (contentPageListParam.PId != 0)
            {
                StatusCodeExpression += $" and PId={contentPageListParam.PId}";
            }
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == contentPageListParam.ModelId);
            if (contentmodel == null)
                return new Page();
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == contentPageListParam.ModelId)).ToList();
            string filed = ColumnContentUpdateFiledConfig.defaultFields;
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
            }
            filed = filed.TrimEnd(',');
            DynamicParameters parameters = new DynamicParameters();
            string swhere = string.Empty;
            _sqlTableServices.CreateDapperColumnContentSelectSql(contentPageListParam, modetype, out swhere, out parameters);

            var result = await _dapperDBContext.PageAsync(
                contentPageListParam.page,
                contentPageListParam.limit,
                "select " + filed.GetCurrentBaseField()
                + " from " + contentmodel.TableName
                + " where " + StatusCodeExpression + swhere + orderby
                , parameters);
            if (modetype == 1)
            {
                expression = expression.And(m => m.ParentModelId == contentPageListParam.ModelId);
                var relationtable = await _unitOfWork.GetRepository<sysTableRelation>().GetAllAsync(expression);
                var relationlist = new List<TableRelationListDto>();
                if (relationtable.Count > 0)
                {
                    relationlist = _mapper.Map<List<TableRelationListDto>>(relationtable);
                    //relationjson = relationlist.ToJson();
                }
                var list = result.Items.ToList();
                list.Each(item =>
                {
                    if ((StatusCode)item.StatusCode == StatusCode.Enable)
                    {
                        if (Convert.ToDateTime(item.PublishTime) > Clock.Now)
                        {
                            item.StatusCode = (object)(int)StatusCode.PendingRelease;
                        }
                    }
                    item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                    item.StatusCodeText = ((StatusCode)item.StatusCode).GetEnumDescription();
                    item.relationInfo = relationlist;
                });
                result.Items = list;
            }
            else
            {
                result.Items.Each(item =>
                {
                    item.StatusColor = ((StatusCode)item.StatusCode).GetEnumColorDescription();
                    item.StatusCodeText = ((StatusCode)item.StatusCode).GetEnumDescription();
                });
            }

            return result;
        }
        public async Task<Page> ListAsync(ContentPageListParamDto contentPageListParam)
        {
            return await AbstractList(contentPageListParam, 1, " order by OrderId desc");
        }

        public async Task<Dictionary<object, object>> GetContentForReviewById(int ParentId, int Id, int ModelId)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return default;
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == ModelId);
            if (contentmodel == null)
                return default;
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == ModelId)).ToList();
            string filed = "ReviewAddUser,MsgGroupId";

            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "Id", Id },
                { "ParentId", ParentId }
            };
            string swhere = string.Empty;
            _sqlTableServices.InitDapperColumnContentSwheresql(ref swhere, ref parameters, dict);
            var result = (await _dapperDBContext.GetDynamicAsync("select " + filed.GetCurrentBaseField() + " from " + contentmodel.TableName + " where " + swhere, parameters)).FirstOrDefault();
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
        public async Task<ProblemDetails<OutputContentAndWorkFlowDto>> GetContentById(int modelId, int ParentId, int Id)
        {
            if (!await CheckPermission(ParentId.ToInt(), nameof(DataPermissionDto.sp)))
                return new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.NotFound, ErrorCodes.DataNotFound.GetEnumDescription());
            var column = await _unitOfWork.GetRepository<SysColumn>().GetFirstOrDefaultAsync(m => m.Id == ParentId);
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == modelId);
            if (contentmodel == null)
                return new ProblemDetails<OutputContentAndWorkFlowDto>(HttpStatusCode.NotFound, ErrorCodes.DataNotFound.GetEnumDescription());
            var fieldmodel = (await _unitOfWork.GetRepository<sysField>().GetAllAsync(m => m.ModelId == modelId)).ToList();
            string filed = ColumnContentUpdateFiledConfig.defaultFields;
            foreach (var item in fieldmodel)
            {
                filed += item.FieldName + ",";
            }
            filed = filed.TrimEnd(',');
            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "Id", Id },
                { "ParentId", ParentId }
            };
            string swhere = string.Empty;
            _sqlTableServices.InitDapperColumnContentSwheresql(ref swhere, ref parameters, dict);

            var result = (await _dapperDBContext.GetDynamicAsync("select " + filed.GetCurrentBaseField() + "," + column.ReviewMode + " as flowId from " + contentmodel.TableName + " where" + swhere, parameters)).FirstOrDefault();
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
        public async Task<ProblemDetails<string>> GetFormHtml(int currentmodelId)
        {
            var contentmodel = await _unitOfWork.GetRepository<SysContentModel>().GetFirstOrDefaultAsync(m => m.Id == currentmodelId);
            if (contentmodel == null)
                return Problem<string>(HttpStatusCode.BadRequest, "没有选择有效模型");
            if (contentmodel.FormHtmlString.IsNullOrEmpty())
                contentmodel.FormHtmlString = "[]";
            return Problem<string>(HttpStatusCode.OK, contentmodel.FormHtmlString);
        }
    }
}
