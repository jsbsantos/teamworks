using System.Linq;
using System.Web.Mvc;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ActivitiesController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int projectId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(int projectId, int activityId)
        {
            RavenQueryStatistics stats;
            ViewBag.Results = DbSession.Query<ActivitiesTotalDuration.Result, ActivitiesTotalDuration>()
                .Customize(c => c.Include<ActivitiesTotalDuration.Result>(r => r.ActivityId))
                .Where(r => r.ActivityId == activityId.ToId("activity")).ToList();

            Activity activity = DbSession.Query<Activity>()
                .Statistics(out stats)
                .Customize(c => c.Include<Activity>(a => a.Project)
                                    .Include<Activity>(a => a.Dependencies))
                .Where(a => a.Id == activityId.ToId("activity")
                            && a.Project == projectId.ToId("project")).FirstOrDefault();

            if (activity == null)
                return HttpNotFound();

            var project = DbSession.Load<Project>(projectId.ToId("project"));

            if (project == null)
                return HttpNotFound();

            var vm = activity.MapTo<ActivityViewModel>();
            vm.ProjectReference = project.MapTo<EntityViewModel>();
            return View(vm);
        }
    }
}