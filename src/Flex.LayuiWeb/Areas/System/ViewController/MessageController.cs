using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "消息页面")]
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SendMsg()
        {
            return View();
        }
    }
}
