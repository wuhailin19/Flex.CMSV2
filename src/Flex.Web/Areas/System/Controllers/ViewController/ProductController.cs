using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.system.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "项目管理页面")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
