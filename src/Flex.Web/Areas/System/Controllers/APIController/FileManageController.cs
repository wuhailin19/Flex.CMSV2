using Flex.Core.Attributes;
using Flex.Domain.Dtos.FileManage;
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
        public string GetFiles(string path = "/")
        {
            var result = _fileManageServices.GetDirectoryByPath(path);
            return Success(result.OrderByDescending(m => m.isdir).ThenBy(m => m.name));
        }

        [HttpPost("CreateDirectory")]
        public async Task<string> CreateDirectory()
        {
            var model = await GetModel<DirectoryQueryDto>();
            var result = _fileManageServices.CreateDirectory(model.path, model.folder);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
