using Flex.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.ViewController
{
    [Area("System")]
    [Descriper(Name = "审核流程图页面")]
    public class WorkFlowController : Controller
    {
        public ActionResult Index() { return View(); }
        public ActionResult DesignFlowchart() { return View(); }
        public ActionResult Edit() { return View(); }
        public ActionResult EditWorkflowManager()
        {
            return View();
        }
        public ActionResult AddWorkFlow()
        {
            return View();
        }
    }
}
