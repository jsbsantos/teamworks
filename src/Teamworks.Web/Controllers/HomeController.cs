using System.Web.Mvc;
using Teamworks.Web.Controllers;
using Teamworks.Web.Controllers.Api;

namespace TeamWorks.Web.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index() {
            return View(ProjectsController.Projects);
        }
    }
}