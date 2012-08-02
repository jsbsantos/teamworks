using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class TimelogController : RavenDbController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }
    }
}