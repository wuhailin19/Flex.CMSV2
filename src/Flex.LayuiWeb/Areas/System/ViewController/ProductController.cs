﻿using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.LayuiWeb.Areas.system.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "项目管理页面")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult AddPage()
        {
            return View();
        }
        public IActionResult EditPage()
        {
            return View();
        }
        public IActionResult AddRecord()
        {
            return View();
        }
    }
}
