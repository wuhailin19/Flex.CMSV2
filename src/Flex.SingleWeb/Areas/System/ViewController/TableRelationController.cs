using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ViewController
{
    [Area("System")]
    [Descriper(Name = "模型关系页面")]
    public class TableRelationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPage()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
