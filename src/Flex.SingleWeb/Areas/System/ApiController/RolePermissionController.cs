using Consul;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "角色权限相关接口")]
    public class RolePermissionController : ApiBaseController
    {
        private IRoleServices _roleServices;
        public RolePermissionController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        [HttpGet("Column")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Column()
        {
            return Success(ModelTools<RoleColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        [Descriper(Name = "角色列表分页数据")]
        public async Task<string> ListAsync(int page, int limit)
        {
            return Success(await _roleServices.GetRoleListAsync(page, limit));
        }
        [HttpGet("PermissionList")]
        [Descriper(Name = "角色下拉数据")]
        public async Task<string> PermissionList()
        {
            return Success(await _roleServices.GetRoleListAsync());
        }

        [HttpGet("StepPermissionList")]
        [Descriper(Name = "工作流页面角色数据")]
        public async Task<string> StepPermissionList()
        {
            return Success(await _roleServices.GetStepRoleListAsync());
        }

        [HttpGet("GetDataPermissionListById")]
        [Descriper(Name = "传入角色Id获取栏目权限列表")]
        public async Task<string> GetDataPermissionListById(int Id)
        {
            var result = await _roleServices.GetDataPermissionListById(Id);
            if (result != null)
                return Success(result);
            else
                return Success(new DataPermissionDto());
        }

        [HttpGet("GetSitePermissionListById")]
        [Descriper(Name = "传入角色Id获取站点权限字符串")]
        public async Task<string> GetSitePermissionListById(int Id)
        {
            var result = await _roleServices.GetSitePermissionListById(Id);
            return Success(result);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        [Descriper(Name = "新增角色")]
        public async Task<string> Insert(InputRoleDto role)
        {
            var result = await _roleServices.AddNewRole(role);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Descriper(Name = "修改角色")]
        public async Task<string> Update(InputUpdateRoleDto role)
        {
            var result = await _roleServices.UpdateRole(role);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改菜单权限
        /// </summary>
        /// <param name="roleMenuDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateMenuPermission")]
        [Descriper(Name = "修改菜单权限")]
        public async Task<string> UpdateMenuPermission(InputRoleMenuDto roleMenuDto)
        {
            var result = await _roleServices.UpdateMenuPermission(roleMenuDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改栏目数据权限
        /// </summary>
        /// <param name="roleMenuDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateDataPermission")]
        [Descriper(Name = "修改栏目数据权限")]
        public async Task<string> UpdateDataPermission(InputRoleDatapermissionDto roledataDto)
        {
            var result = await _roleServices.UpdateDataPermission(roledataDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改站点数据权限
        /// </summary>
        /// <param name="roleMenuDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateSitePermission")]
        [Descriper(Name = "修改站点数据权限")]
        public async Task<string> UpdateSitePermission(InputRoleSitepermissionDto roledataDto)
        {
            var result = await _roleServices.UpdateSitePermission(roledataDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        /// <summary>
        /// 修改接口权限
        /// </summary>
        /// <param name="roleMenuDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateApiPermission")]
        [Descriper(Name = "修改接口权限")]
        public async Task<string> UpdateApiPermission(InputRoleUrlDto roleurlDto)
        {
            var result = await _roleServices.UpdateApiPermission(roleurlDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }


        [HttpPost("{Id}")]
        [Descriper(Name = "删除角色")]
        public async Task<string> Delete(string Id)
        {
            var result = await _roleServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
