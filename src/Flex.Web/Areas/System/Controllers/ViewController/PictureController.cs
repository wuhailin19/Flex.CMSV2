using Flex.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "菜单页面")]
    public class PictureController : Controller
    {
        public IActionResult Index() { 
            return View();
        }
    }
}
