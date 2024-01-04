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
        Task<dynamic> GetContentById(int ParentId, int Id);
        Task<string> GetFormHtml(int ParentId);
        Task<IEnumerable<ModelTools<ColumnContentDto>>> GetTableThs(int ParentId);
        Task<Page> ListAsync(int pageindex, int pagesize, int ParentId);
        Task<ProblemDetails<string>> Update(Hashtable table);
    }
}
