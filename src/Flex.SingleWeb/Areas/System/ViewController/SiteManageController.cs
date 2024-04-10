using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "站点管理页面")]
    public class SiteManageController : Controller
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
