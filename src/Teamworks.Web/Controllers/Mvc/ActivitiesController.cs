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

            /*
            var cprojects = new Api.ProjectsController(DbSession);
            var cactivities = new Api.ActivitiesController(DbSession);

            ViewBag.Project = cprojects.Get(projectId);
            if (identifier == null)
            {
                var activities = cactivities.Get(projectId);
                return View("Activities", activities);
            }

            var tlController = new Api.TimeController(DbSession);
            ViewBag.Timelogs = tlController.Get(projectId, identifier.Value);

            return View("Activity", cactivities.Get(identifier.Value, projectId));
            */
        }
    }
}