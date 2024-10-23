using Microsoft.AspNetCore.Mvc;

namespace Flex.SingleWeb.Areas.System.ViewController
{
    [Area("System")]
    public class OauthController : Controller
    {
        public IActionResult Callback()
        {
            return View();
        }
    }
}
