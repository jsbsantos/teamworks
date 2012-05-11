using System.Web.Mvc;
using Teamworks.Web.Controllers;
using Teamworks.Web.Controllers.Base;

namespace TeamWorks.Web.Controllers {
    public class HomeController : RavenController {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index() {
            return View();
        }
    }
}