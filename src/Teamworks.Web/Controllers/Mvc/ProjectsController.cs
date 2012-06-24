using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier)
        {
            var model = "/api/projects/";
            return identifier != null ? 
                View("Project", (object) (model + identifier)) 
                : View("Projects", (object) model);
        }
    }
}