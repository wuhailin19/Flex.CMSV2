﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    public class ColumnContentController : Controller
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
        public IActionResult Manage()
        {
            return View();
        }
    }
}