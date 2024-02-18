using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "消息页面")]
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
