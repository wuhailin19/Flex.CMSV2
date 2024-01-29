using Consul;
using Flex.Domain.Dtos.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ApiBaseController
    {
        private IRoleServices _roleServices;
        public RolePermissionController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        [HttpGet("Column")]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<RoleColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync(int page, int limit)
        {
            return Success(await _roleServices.GetRoleListAsync(page, limit));
        }
        [HttpGet("PermissionList")]
        public async Task<string> PermissionList()
        {
            return Success(await _roleServices.GetRoleListAsync());
        }

        [HttpGet("GetDataPermissionListById")]
        public async Task<string> GetDataPermissionListById(int Id)
        {
            var result = await _roleServices.GetDataPermissionListById(Id);
            if (result.Count() > 0)
                return Success(result);
            else
                return Fail("无数据");
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<string> Insert(InputRoleDto role) {
            var result = await _roleServices.AddNewRole(role);
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
        public async Task<string> UpdateMenuPermission(InputRoleMenuDto roleMenuDto) {
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
        public async Task<string> UpdateDataPermission(InputRoleDatapermissionDto roledataDto) {
            var result = await _roleServices.UpdateDataPermission(roledataDto);
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
        public async Task<string> UpdateApiPermission(InputRoleUrlDto roleurlDto)
        {
            var result = await _roleServices.UpdateApiPermission(roleurlDto);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }


        [HttpDelete("{Id}")]
        public async Task<string> Delete(string Id)
        {
            var result = await _roleServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
