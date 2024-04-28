using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ViewController
{
    [Area("System")]
    [Descriper(Name = "文件管理页面")]
    public class SystemLogController : Controller
    {
        [Descriper(Name = "日志管理页面")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
