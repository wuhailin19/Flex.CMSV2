using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain;
using Flex.Domain.Dtos.Column;
using Flex.Domain.Dtos.ColumnContent;
using System.Collections;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnContentServices
    {
        Task<ProblemDetails<string>> Add(Hashtable hashtable);
        Task<ProblemDetails<string>> Delete(int ParentId, string Id);
        Task<OutputContentAndWorkFlowDto> GetContentById(int ParentId, int Id);
        Task<IEnumerable<ContentOptions>> GetContentOptions(int ParentId);
        Task<ProblemDetails<string>> GetFormHtml(int ParentId);
        Task<ColumnPermissionAndTableHeadDto> GetTableThs(int ParentId);
        Task<Page> ListAsync(int pageindex, int pagesize, int ParentId);
        Task<ProblemDetails<string>> Update(Hashtable table);
    }
}
