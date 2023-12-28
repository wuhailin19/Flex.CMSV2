using Flex.Domain.Dtos.IndexShortCut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    
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
        public async Task<string> GetMenuShortcut(string mode = "1")
        {
            var result = await _menuServices.getMenuShortcutAsync(mode);
            if (result.IsNullOrEmpty())
                return Fail("");
            return Success(result);
        }
      
        [HttpPost("Update")]
        public async Task<string> Update([FromForm] ShortCutDtos shortCutDtos)
        {
            if (await _systemIndexSetServices.UpdateCurrentAsync(shortCutDtos) > 0)
                return Success("操作成功");
            else
                return Fail("操作失败");
        }
        [HttpPost("Delete")]
        public async Task<string> Delete([FromForm] ShortCutDtos shortCutDtos)
        {
            if (await _systemIndexSetServices.DeleteCurrentAsync(shortCutDtos) > 0)
                return Success("操作成功");
            else
                return Fail("操作失败");
        }
    }
}
