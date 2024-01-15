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
        public UploadController(IUploadServices uploadServices)
        {
            this.uploadServices = uploadServices;
        }
        [HttpGet("Config")]
        [AllowAnonymous]
        public string Config()
        {
            var result =  uploadServices.Config();
            if (result.IsNullOrEmpty())
                return Fail("");
            return result;
        }
    }
}
