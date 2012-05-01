using System.Web.Mvc;

namespace Teamworks.Web.Controllers {
    public class ProjectsController : Controller {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id) {
            return View();
        }
    }
}