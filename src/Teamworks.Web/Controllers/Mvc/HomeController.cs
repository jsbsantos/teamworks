using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class HomeController : RavenController
    {
        [HttpGet]
        [AllowAnonymous]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }
    }
}