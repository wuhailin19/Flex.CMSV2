using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "站点管理页面")]
    public class SiteManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
