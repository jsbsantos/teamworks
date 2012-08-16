using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class HomeController : RavenController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            return View("Welcome");
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}