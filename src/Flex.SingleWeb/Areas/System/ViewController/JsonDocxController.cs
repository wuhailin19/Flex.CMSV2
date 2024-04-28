using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ViewController
{
    [Area("System")]
    [Descriper(Name = "栏目Json文档")]
    public class JsonDocxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ColumnApi()
        {
            return View();
        }
    }
}
