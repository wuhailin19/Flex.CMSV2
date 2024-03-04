using Flex.Core.Attributes;
using Flex.Core.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "文件管理器接口")]
    public class FileManageController : ApiBaseController
    {
        private IFileManageServices _fileManageServices;
        public FileManageController(IFileManageServices fileManageServices)
        {
            _fileManageServices = fileManageServices;
        }
        [HttpGet("GetFiles")]
        [AllowAnonymous]
        public string GetFiles(string path="/") {
            var result = _fileManageServices.GetDirectoryByPath(path);
            return Success(result.OrderByDescending(m=>m.isdir).ThenBy(m=>m.name));
        }
    }
}
