using System.Linq;
using System.Web.Mvc;
using Raven.Client.Linq;
using Teamworks.Core.Services.RavenDb.Indexes;

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

        public ActionResult Welcome()
        {
            return View();
        }

    }
}