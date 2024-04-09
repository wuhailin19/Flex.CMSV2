using Flex.Core.Attributes;
using Flex.Core.Timing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.WebApi.SystemControllers
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
                return default;
            }
        }
        [HttpGet("getDataStr")]
        [AllowAnonymous]
        public DateTime getDataStr()
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
            return localTime;
        }
    }
}
