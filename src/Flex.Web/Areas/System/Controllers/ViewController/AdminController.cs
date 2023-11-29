using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    public class AdminViewController : Controller
    {
        public IActionResult Edit()
        {
            return View();
        }
    }
}
