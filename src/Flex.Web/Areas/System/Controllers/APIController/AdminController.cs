using Flex.Application.Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ApiBaseController
    {
        private IRoleServices _roleServices;
        public AdminController(IRoleServices roleServices) {
            _roleServices = roleServices;
        }
        /// <summary>
        /// 根据Token获取当前角色权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("Permissions")]
        public async Task<string> GetCurrenAdminPermissions()
        {
            var RoleList = await _roleServices.GetCurrentAdminRoleByTokenAsync();
            if (!RoleList.Any())
                return NotFound();
            return Success(RoleList);
        }
    }
}
