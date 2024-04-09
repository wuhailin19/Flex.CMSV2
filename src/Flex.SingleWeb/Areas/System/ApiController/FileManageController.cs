using Flex.Core.Attributes;
using Flex.Domain.Dtos.FileManage;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
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
        [Descriper(Name = "获取当前目录所有文件")]
        public string GetFiles(string path = "/")
        {
            var result = _fileManageServices.GetDirectoryByPath(path);
            return Success(result.OrderByDescending(m => m.isdir).ThenBy(m => m.name));
        }

        [HttpPost("CreateDirectory")]
        [Descriper(Name = "新建文件夹")]
        public async Task<string> CreateDirectory()
        {
            var model = await GetModel<DirectoryQueryDto>();
            var result = _fileManageServices.CreateDirectory(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("ChangeDirectory")]
        [Descriper(Name = "更改文件目录")]
        public async Task<string> ChangeDirectory()
        {
            var model = await GetModel<ChangeDirectoryQueryDto>();
            var result = _fileManageServices.ChangeDirectory(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail, result.Status);
        }

        [HttpPost("RenameDirectoryorFile")]
        [Descriper(Name = "更改文件名称")]
        public async Task<string> RenameDirectoryorFile()
        {
            var model = await GetModel<RenameDirectoryQueryDto>();
            var result = _fileManageServices.RenameDirectoryorFile(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }

        [HttpPost("ChangeFileContent")]
        [Descriper(Name = "更改文件内容")]
        public async Task<string> ChangeFileContent()
        {
            var model = await GetModel<ChangeFileContentQueryDto>();
            var result = _fileManageServices.ChangeFileContent(model);
            if (result.IsSuccess)
                return Success(result.Detail);
            return Fail(result.Detail);
        }
    }
}
