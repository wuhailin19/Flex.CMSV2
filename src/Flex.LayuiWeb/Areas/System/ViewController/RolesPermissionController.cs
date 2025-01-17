﻿using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "角色页面")]
    public class RolesPermissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPage()
        {
            return View();
        }
        public ActionResult MenuEdit()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult DataPermission()
        {
            return View();
        }
        public ActionResult ApiPermission()
        {
            return View();
        }
    }
}
