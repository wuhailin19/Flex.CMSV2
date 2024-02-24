using Flex.Core.Attributes;
using Flex.Domain.Dtos.IndexShortCut;
using Flex.Domain.Dtos.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "首页快捷方式相关接口")]
    public class SystemIndexController : ApiBaseController
    {
        private IMenuServices _menuServices;
        private ISystemIndexSetServices _systemIndexSetServices;
        public SystemIndexController(IMenuServices menuServices, ISystemIndexSetServices systemIndexSetServices)
        {
            _menuServices = menuServices;
            _systemIndexSetServices = systemIndexSetServices;
        }
        [HttpGet("getMenuShortcut")]
        [Descriper(Name = "获取快捷方式")]
        public async Task<string> GetMenuShortcut(string mode = "1")
        {
            var result = await _menuServices.getMenuShortcutAsync(mode);
            if (result == null)
                result = new List<MenuColumnDto>();
            return Success(result);
        }
      
        [HttpPost("Update")]
        [Descriper(Name = "修改快捷方式")]
        public async Task<string> Update()
        {
            var shortCutDtos =await GetModel<ShortCutDtos>();
            if (await _systemIndexSetServices.UpdateCurrentAsync(shortCutDtos) > 0)
                return Success("操作成功");
            else
                return Fail("操作失败");
        }
        [HttpPost("Delete")]
        [Descriper(Name = "删除快捷方式")]
        public async Task<string> Delete()
        {
            var shortCutDtos = await GetModel<ShortCutDtos>();
            if (await _systemIndexSetServices.DeleteCurrentAsync(shortCutDtos) > 0)
                return Success("操作成功");
            else
                return Fail("操作失败");
        }
    }
}
