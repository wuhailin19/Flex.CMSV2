using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities.System;

namespace Flex.Application.Contracts.IServices
{
    public interface IRoleServices
    {
        Task<IEnumerable<RoleDto>> GetCurrentAdminRoleByTokenAsync();
        Task<IEnumerable<SysRole>> GetRoleByRoleIdAsync(string roleId);
        Task<PagedList<RoleColumnDto>> GetRoleListAsync(int page, int pagesize);
        Task<Dictionary<string, List<string>>> PermissionDtosAsync();
    }
}
