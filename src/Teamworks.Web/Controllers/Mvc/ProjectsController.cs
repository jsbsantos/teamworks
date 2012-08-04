using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        //[ActionName("View")]
        public ActionResult Index(int? identifier)
        {

            /*
            const string endpoint = "/api/projects/";
            if (identifier != null)
            {
                ViewBag.Endpoint = endpoint + identifier;
                return View("Project");
            }
            ViewBag.Endpoint = endpoint;
            return View("Projects");
             */
            return null;
        }

        [HttpGet]
        [ActionName("View")]
        public ActionResult IndexNew(int? identifier)
        { 
            // todo change name
            var pController = new Api.ProjectsController(DbSession);
            if (identifier.HasValue)
            {
                var aController = new Api.ActivitiesController(DbSession);
                ViewBag.Activities = aController.Get(identifier.Value);


                return View("ProjectNew", pController.Get(identifier.Value));
            }
            return View("ProjectsNew", pController.Get());
        }
    }
}