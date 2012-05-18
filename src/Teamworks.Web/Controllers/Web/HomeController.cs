using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Web
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