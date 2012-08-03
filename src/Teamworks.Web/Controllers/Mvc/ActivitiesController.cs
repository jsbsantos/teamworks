using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ActivitiesController : RavenController
    {
        
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier, int projectid)
        {
            var endpoint = "/api/projects/" + projectid + "/activities/";
            if (identifier != null)
            {
                ViewBag.Endpoint = endpoint + identifier;
                return View("Activity");
            }
            ViewBag.Endpoint = endpoint;
            return View("Activities");
        }
    }
}