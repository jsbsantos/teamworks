using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class TimelogController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }
    }
}