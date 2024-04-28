using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "栏目管理页面")]
    public class ColumnCategoryController : Controller
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
