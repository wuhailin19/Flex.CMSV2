using Flex.Core.Attributes;
using Flex.Domain.Dtos.IndexShortCut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]

    public class UploadController : ApiBaseController
    {
        IUploadServices uploadServices;
        private IPictureServices _pictureServices;
        public UploadController(IUploadServices uploadServices, IPictureServices pictureServices)
        {
            this.uploadServices = uploadServices;
            _pictureServices = pictureServices;
        }
        [HttpGet("Config")]
        [AllowAnonymous]
        public string Config()
        {
            var result = uploadServices.Config();
            if (result.IsNullOrEmpty())
                return Fail("");
            return result;
        }
        [HttpGet("TestOptions")]
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
       
        [HttpPost("OnLoad")]
        [Descriper(Name = "上传图片")]
        public string OnLoad(IFormFileCollection file)
        {
            var result = _pictureServices.UploadImgService(file);
            if (!result.IsSuccess)
                return Fail(result.Detail);
            return Success(result.Detail);
        }
    }

    public class Options { 
        public string text { set; get; }
        public string value { set; get; }
        public bool @checked { set; get; }
    }
}
