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
        public string getDataStr()
        {
            //TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            //DateTime utcTime = DateTime.SpecifyKind(Clock.Now, DateTimeKind.Utc);
            //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
            var date = Clock.Now.ToString("yyyy年MM月dd日")+" "+ ChineseByEnWeek(Clock.Now);
            return Success(date);
        }

        private string ChineseByEnWeek(DateTime dt)
        {
            switch (dt.DayOfWeek.ToString())
            {
                case "Monday": return "星期一";
                case "Tuesday": return "星期二";
                case "Wednesday": return "星期三";
                case "Thursday": return "星期四";
                case "Friday": return "星期五";
                case "Saturday": return "星期六";
                case "Sunday": return "星期日";
                default: return "";
            }
        }
    }
}
