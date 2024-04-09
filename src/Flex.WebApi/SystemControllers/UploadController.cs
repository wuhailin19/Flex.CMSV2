using Flex.Core.Attributes;
using Flex.Core.Config;
using Flex.Domain.Dtos.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "上传接口")]
    public class UploadController : ApiBaseController
    {
        IUploadServices uploadServices;
        private IPictureServices _pictureServices;
        private IFileServices _fileServices;
        public UploadController(IUploadServices uploadServices, IPictureServices pictureServices, IFileServices fileServices)
        {
            this.uploadServices = uploadServices;
            _pictureServices = pictureServices;
            _fileServices = fileServices;
        }

        [HttpGet("Config")]
        [AllowAnonymous]
        [Descriper(IsFilter = true)]
        public string Config()
        {
            var result = uploadServices.Config();
            if (result.IsNullOrEmpty())
                return Fail("");
            return result;
        }
        [HttpPost("UploadFile")]
        [Descriper(Name = "上传文件")]
        public string UploadFile(IFormFileCollection file)
        {
            var result = _fileServices.UploadFilesService(file);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(ServerConfig.FileServerUrl + result.Detail);
        }

        [HttpPost("UploadFilesToPath")]
        [Descriper(Name = "上传文件")]
        public string UploadFilesToPath([FromForm] UploadFileToPathDto uploadFileToPathDto)
        {
            var result = _fileServices.UploadFilesToPathService(uploadFileToPathDto.file, uploadFileToPathDto.path);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(ServerConfig.FileServerUrl + result.Detail);
        }

        [HttpPost("UploadImage")]
        [Descriper(Name = "上传图片")]
        public string UploadImage(IFormFileCollection file)
        {
            var result = _pictureServices.UploadImgService(file);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(ServerConfig.ImageServerUrl + result.Detail);
        }
        [HttpPost("UploadPasteImage")]
        [Descriper(Name = "上传粘贴板图片")]
        public string UploadPasteImage(IFormFileCollection upfile)
        {
            var result = _pictureServices.UploadImgService(upfile);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
        [HttpPost("UploadRemoteImage")]
        [Descriper(Name = "上传远程图片")]
        public async Task<string> UploadRemoteImage([FromForm] string[] upfile)
        {
            var result = await _pictureServices.UploadRemoteImage(upfile);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result);
        }
    }

    public class Options
    {
        public string text { set; get; }
        public string value { set; get; }
        public bool @checked { set; get; }
    }
}
