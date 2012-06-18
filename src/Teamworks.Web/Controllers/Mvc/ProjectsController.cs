using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id)
        {
            return id != null ? View("Project", id.GetValueOrDefault(0)) : View();
        }
    }
}