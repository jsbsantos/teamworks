using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client.Linq;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier)
        {
            const string endpoint = "/api/projects/";
            if (identifier != null)
            {
                ViewBag.Endpoint = endpoint + identifier;

                var project = DbSession
                    .Load<Core.Project>(identifier);

                var act = DbSession.Query
                    <Core.Services.RavenDb.Indexes.Activities_Duration_Result,
                        Core.Services.RavenDb.Indexes.Activities_Duration>()
                    .Where(a => a.Project == project.Id)
                    .OrderBy(a => a.StartDate)
                    .Select(x => new
                                     {
                                         x.Dependencies,
                                         x.Description,
                                         x.Duration,
                                         x.Id,
                                         x.Name,
                                         x.Project,
                                         x.StartDate,
                                         x.TimeUsed,
                                         AccumulatedTime = x.StartDate.Subtract(project.StartDate).TotalMinutes
                                     })
                    .ToList();

                ViewBag.ChartData = JsonConvert.SerializeObject(act);

                return View("Project");
            }
            ViewBag.Endpoint = endpoint;

            return View("Projects");
        }

        [HttpGet]
        //[ActionName("View")]
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