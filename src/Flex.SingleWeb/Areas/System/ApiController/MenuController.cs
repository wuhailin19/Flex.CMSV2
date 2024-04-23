using Flex.Application.Contracts.IServices;
using Flex.Application.Services;
using Flex.Core.Attributes;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.Menu;
using Flex.Domain.Dtos.System.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "菜单相关接口")]
    public class MenuController : ApiBaseController
    {
        private IMenuServices _menuServices;
        public MenuController(IMenuServices menuServices)
        {
            _menuServices = menuServices;
        }
        [HttpGet("getMainMenuStrAsync")]
        [Descriper(Name = "主页面左侧树状菜单数据")]
        public async Task<string> getMainMenuStrAsync()
        {
            var list = await _menuServices.GetMainMenuDtoAsync();
            if (list.IsNullOrEmpty())
                return NotFound();
            return Success(list);
        }

        [HttpGet("Column")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Column()
        {
            return Success(ModelTools<MenuColumnDto>.getColumnDescList());
        }
        [HttpGet("ListAsync")]
        [Descriper(Name = "菜单管理页面数据")]
        public async Task<string> ListAsync()
        {
            return Success((await _menuServices.GetMenuListAsync()).Where(m => m.ParentID != 0).OrderBy(m => m.OrderId).ThenBy(m => m.ID));
        }
        [HttpGet("GetTreeMenuAsync")]
        [Descriper(Name = "菜单编辑页面父级数据")]
        public async Task<string> GetMenuListAsync()
        {
            var result = await _menuServices.GetTreeMenuAsync();
            return Success(result);
        }

        [HttpGet("TreeListAsync")]
        [Descriper(Name = "获取当前角色可操作的菜单数据")]
        public async Task<string> TreeListAsync()
        {
            return Success(await _menuServices.GetCurrentMenuDtoByRoleIdAsync());
        }

        [HttpGet("GetTreeListByIdAsync/{Id}")]
        [Descriper(Name = "通过角色Id获取可操作的菜单数据")]
        public async Task<string> GetTreeListByIdAsync(int Id)
        {
            return Success(await _menuServices.GetMenuDtoByRoleIdAsync(Id));
        }

        [HttpPost]
        [Descriper(Name = "编辑菜单")]
        public async Task<string> Update()
        {
            var model = await GetModel<MenuEditDto>();
            var result = await _menuServices.EditMenu(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("QuickEdit")]
        [Descriper(Name = "快速修改菜单状态")]
        public async Task<string> QuickEdit()
        {
            var model = await ValidateModel<MenuQuickEditDto>();
            if(!model.IsSuccess)
                return Fail(model.Detail);
            var result = await _menuServices.QuickEditMenu(model.Content);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
        [HttpPost("CreateMenu")]
        [Descriper(Name = "新增菜单")]
        public async Task<string> AddMenu()
        {
            var model = await GetModel<MenuAddDto>();
            var result = await _menuServices.AddMenu(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("{Id}")]
        [Descriper(Name = "删除菜单")]
        public async Task<string> Delete(string Id)
        {
            var result = await _menuServices.Delete(Id);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
