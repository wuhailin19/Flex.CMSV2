using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "登录页面")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }
    }
}
