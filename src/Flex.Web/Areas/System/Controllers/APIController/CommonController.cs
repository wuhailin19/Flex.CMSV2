using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    [Descriper(IsFilter = true)]
    public class CommonController : ApiBaseController
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "Scripts/iconfont/iconfont.ttf";
        /// <summary>
        /// 文件转byte
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public byte[] getBufferArray()
        {
            try
            {
                byte[] byteArray = FileBinaryConvertHelper.File2Bytes(path);
                return byteArray;
            }
            catch (Exception ex)
            {
                return default(byte[]);
            }
        }
    }
}
