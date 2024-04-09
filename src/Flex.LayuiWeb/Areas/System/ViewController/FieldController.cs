using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "模型字段页面")]
    public class FieldController : Controller
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
