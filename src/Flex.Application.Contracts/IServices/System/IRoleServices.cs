using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Role;
using Flex.Domain.Entities.System;

namespace Flex.Application.Contracts.IServices
{
    public interface IRoleServices
    {
        Task<ProblemDetails<string>> AddNewRole(InputRoleDto role);
        Task<Dictionary<int, List<string>>> CurrentUrlPermissionDtosAsync();
        Task<ProblemDetails<string>> Delete(string Id);
        Task<IEnumerable<RoleDto>> GetCurrentAdminRoleByTokenAsync();
        Task<SysRole> GetCurrentRoldDtoAsync();
        Task<DataPermissionDto> GetDataPermissionListById(int Id);
        Task<SysRole> GetRoleByIdAsync(int Id);
        Task<IEnumerable<SysRole>> GetRoleByRoleIdAsync(int roleId);
        Task<PagedList<RoleColumnDto>> GetRoleListAsync(int page, int pagesize);
        Task<IEnumerable<RoleSelectDto>> GetRoleListAsync();
        Task<string> GetSitePermissionListById(int Id);
        Task<IEnumerable<RoleStepDto>> GetStepRoleListAsync();
        Task<Dictionary<string, List<string>>> PermissionDtosAsync();
        Task<ProblemDetails<string>> UpdateApiPermission(InputRoleUrlDto role);
        Task<ProblemDetails<string>> UpdateDataPermission(InputRoleDatapermissionDto role);
        Task<ProblemDetails<string>> UpdateMenuPermission(InputRoleMenuDto role);
        Task<ProblemDetails<string>> UpdateRole(InputUpdateRoleDto role);
        Task<ProblemDetails<string>> UpdateSitePermission(InputRoleSitepermissionDto role);
    }
}
