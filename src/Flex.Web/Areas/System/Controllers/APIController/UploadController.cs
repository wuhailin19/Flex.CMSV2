using Flex.Core.Attributes;
using Flex.Core.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    [Descriper(Name = "上传接口")]
    public class UploadController : ApiBaseController
    {
        IUploadServices uploadServices;
        private IPictureServices _pictureServices;
        private IFileServices _fileServices;
        private IFileManageServices _fileManageServices;
        public UploadController(IUploadServices uploadServices, IPictureServices pictureServices, IFileServices fileServices, IFileManageServices fileManageServices)
        {
            this.uploadServices = uploadServices;
            _pictureServices = pictureServices;
            _fileServices = fileServices;
            _fileManageServices = fileManageServices;
        }
        [HttpGet("GetFiles")]
        [AllowAnonymous]
        public string GetFiles(string path) {
            //IFileManager fileManager = new ServiceCollection()
            //  .AddSingleton<IFileProvider>(new HttpFileProvider("http://localhost:3721/files/dir1"))
            //  .AddSingleton<IFileManager, FileManager>()
            //  .BuildServiceProvider()
            //  .GetService<IFileManager>();
            var result = _fileManageServices.GetDirectoryByPath(path);
            return Success(result);
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
        [HttpGet("TestOptions")]
        [Descriper(IsFilter = true)]
        public string TestOptions()
        {
            List<Options> options = new List<Options>
            {
                new Options()
                {
                    text = "1",
                    value = "Test",
                    @checked = false,
                }
            };
            return Success(options);
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
    }

    public class Options
    {
        public string text { set; get; }
        public string value { set; get; }
        public bool @checked { set; get; }
    }
}
