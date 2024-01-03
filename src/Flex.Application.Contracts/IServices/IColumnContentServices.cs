using Flex.Domain;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnContentServices
    {
        Task<string> GetFormHtml(int ParentId);
        Task<Page> ListAsync(int pageindex, int pagesize, int ParentId);
    }
}
