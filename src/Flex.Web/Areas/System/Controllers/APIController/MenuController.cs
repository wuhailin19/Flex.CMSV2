using Flex.Application.Contracts.IServices;
using Flex.Application.Services;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Admin;
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
        [HttpGet("GetTreeMenuAsync")]
        public async Task<string> GetMenuListAsync()
        {
            var result = await _menuServices.GetTreeMenuAsync();
            return Success(result);
        }

        [HttpGet("TreeListAsync")]
        public async Task<string> TreeListAsync()
        {
            return Success(await _menuServices.GetCurrentMenuDtoByRoleIdAsync());
        }

        [HttpPost("Update")]
        public async Task<string> Update()
        {
            var model = await GetModel<MenuEditDto>();
            var result = await _menuServices.EditMenu(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
