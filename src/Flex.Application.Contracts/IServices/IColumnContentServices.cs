using Flex.Domain;

namespace Flex.Application.Contracts.IServices
{
    public interface IColumnContentServices
    {
        Task<Page> ListAsync(int pageindex, int pagesize, int ParentId);
    }
}
