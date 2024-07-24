using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ViewController
{
    [Area("System")]
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
