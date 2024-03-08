using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.WorkFlow;
using Flex.Domain.Entities;
using System.Collections;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnContentServices
    {
        Task<ProblemDetails<int>> Add(Hashtable hashtable, bool IsReview = false);
        Task<ProblemDetails<string>> Delete(int ParentId, string Id);
        Task<OutputContentAndWorkFlowDto> GetButtonListByParentId(int ParentId);
        Task<ProblemDetails<OutputContentAndWorkFlowDto>> GetContentById(int ParentId, int Id);
        Task<Dictionary<object, object>> GetContentForReviewById(int ParentId, int Id);
        Task<IEnumerable<ContentOptions>> GetContentOptions(int ParentId);
        Task<ProblemDetails<string>> GetFormHtml(int ParentId);
        Task<ColumnPermissionAndTableHeadDto<HistoryColumnDto>> GetHistoryTableThs(int ParentId);
        Task<SysContentModel> GetSysContentModelByColumnId(int ParentId);
        Task<ColumnPermissionAndTableHeadDto<ColumnContentDto>> GetTableThs(int ParentId);
        Task<Page> HistoryListAsync(ContentPageListParamDto contentPageListParam);
        Task<Page> ListAsync(ContentPageListParamDto contentPageListParam);
        Task<ProblemDetails<int>> Update(Hashtable table, bool IsReview = false, List<string> white_fileds = null, bool IsCancelReview = false);
        Task<ProblemDetails<int>> SimpleUpdateContent(Hashtable table, bool IsReview = true, List<string> white_fileds = null);
        Task<ProblemDetails<int>> UpdateReviewContent(Hashtable table, bool IsReview = true, bool IsCancelReview = false);
    }
}
