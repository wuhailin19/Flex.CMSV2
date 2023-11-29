using Flex.Application.Contracts.IServices;
using Flex.Domain.Dtos.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ApiBaseController
    {
        private IMenuServices _menuServices;
        public MenuController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }
        [HttpGet("getMainMenuStrAsync")]
        public async Task<string> getMainMenuStrAsync()
        {
            var list = await _menuServices.GetMainMenuDtoAsync();
            if (list.IsNullOrEmpty())
                return NotFound();
            return Success(list);
        }
        
        [HttpGet("Column")]
        [AllowAnonymous]
        public string Column()
        {
            return Success(ModelTools<MenuColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        public async Task<string> ListAsync()
        {
            return Success((await _menuServices.GetMenuListAsync()).Where(m => m.ParentID != 0));
        }

        [HttpGet("TreeListAsync/{Id}")]
        public async Task<string> TreeListAsync(long Id)
        {
            return Success(await _menuServices.GetTreeMenuDtoByRoleIdAsync(Id));
        }

    }
}
