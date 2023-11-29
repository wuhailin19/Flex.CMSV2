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
        
    }
}
