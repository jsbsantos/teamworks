using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenDbController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier)
        {
            const string endpoint = "/api/projects/";
            if (identifier != null)
            {
                ViewBag.Endpoint = endpoint + identifier;
                return View("Project");
            }
            ViewBag.Endpoint = endpoint;
            return View("Projects");
        }
    }
}