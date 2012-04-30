using System.Web.Mvc;

namespace TeamWorks.Web.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index() {
            return View();
        }
    }
}