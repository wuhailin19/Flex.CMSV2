using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult SimpleEdit()
        {
            return View();
        }
        public IActionResult EditPassword()
        {
            return View();
        }
        public IActionResult AddPage()
        {
            return View();
        }
    }
}
