using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier)
        {
            return identifier != null ? View("Project", identifier.Value) : View();
        }
    }
}