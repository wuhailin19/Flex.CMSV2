using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.LayuiWeb.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "栏目内容页面")]
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
        public IActionResult Hishistorical()
        {
            return View();
        }
        public IActionResult SoftDelete()
        {
            return View();
        }
        public IActionResult RestoreContent()
        {
            return View();
        }
        public IActionResult ApprovalProcess()
        {
            return View();
        }
        public IActionResult ContentTools()
        {
            return View();
        }
    }
}
