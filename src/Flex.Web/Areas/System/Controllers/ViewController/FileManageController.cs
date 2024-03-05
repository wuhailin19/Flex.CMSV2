using Flex.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "文件管理页面")]
    public class FileManageController : Controller
    {
        public IActionResult Index() { 
            return View();
        }
        public IActionResult Preview() { 
            return View();
        }
    }
}
