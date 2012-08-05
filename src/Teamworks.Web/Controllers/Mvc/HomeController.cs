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
            if (Request.IsAuthenticated)
            {
                return View();
            }
            return View("Welcome");
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

    }
}