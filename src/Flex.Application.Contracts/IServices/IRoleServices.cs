using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities.System;

namespace Flex.Application.Contracts.IServices
{
    public interface IRoleServices
    {
        Task<ProblemDetails<string>> AddNewRole(InputRoleDto role);
        Task<IEnumerable<RoleDto>> GetCurrentAdminRoleByTokenAsync();
        Task<IEnumerable<SysRole>> GetRoleByRoleIdAsync(string roleId);
        Task<PagedList<RoleColumnDto>> GetRoleListAsync(int page, int pagesize);
        Task<IEnumerable<RoleSelectDto>> GetRoleListAsync();
        Task<Dictionary<string, List<string>>> PermissionDtosAsync();
        Task<ProblemDetails<string>> UpdateMenuPermission(InputRoleMenuDto role);
    }
}
