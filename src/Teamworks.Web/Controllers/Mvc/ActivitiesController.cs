using System.Collections.Generic;
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
            var query = DbSession.Query<Timelog_Filter.Result, Timelog_Filter>()
                .Customize(c =>
                               {
                                   c.WaitForNonStaleResults();
                                   c.Include<Timelog_Filter.Result>(r => r.Activity);
                                   c.Include<Timelog_Filter.Result>(r => r.ActivityDependencies);
                                   c.Include<Timelog_Filter.Result>(r => r.Person);
                                   c.Include<Timelog_Filter.Result>(r => r.Project);
                               })
                .Where(a => a.Project == projectId.ToId("project") && a.Activity == activityId.ToId("activity"))
                .OrderByDescending(r => r.Date)
                .AsProjection<Timelog_Filter.Result>()
                .ToList();

            RavenQueryStatistics stats;
            var activity = DbSession.Query<Activity>()
                .Include(a => a.Project)
                .Statistics(out stats)
                .FirstOrDefault(a => a.Id == activityId.ToId("activity") && a.Project == projectId.ToId("project"));

            if (activity == null)
                return HttpNotFound();

            var project = DbSession.Load<Project>(projectId.ToId("project"));

            if (project == null)
                return HttpNotFound();

            var vm = activity.MapTo<ActivityViewModelComplete>();

            vm.ProjectReference = project.MapTo<EntityViewModel>();

            vm.TotalTimeLogged = query.Sum(r => r.Duration);
            
            //vm.Dependencies =
            //    DbSession.Load<Activity>(activity.Dependencies).Select(a => a.MapTo<ActivityViewModel>()).ToList();

            vm.AssignedPeople =
                DbSession.Load<Person>(activity.People.Distinct()).Select(
                    r => r.MapTo<PersonViewModel>()).ToList();

            vm.Timelogs = query.Select(r =>
                                           {
                                               var result = r.MapTo<TimelogViewModel>();
                                               result.Person = DbSession.Load<Person>(r.Person).MapTo<EntityViewModel>();
                                               return result;
                                           }).ToList();
            ViewBag.Results = vm;

           var list = DbSession.Query<Activity>().Where(r => r.Project == projectId.ToId("project")).ToList();
           vm.Dependencies = list.Select(r =>
                                             {
                                                 var result = r.MapTo<DependencyActivityViewModel>();
                                                 result.Dependency = r.Id.In(activity.Dependencies);
                                                 return result;
                                             })
                .ToList();
            return View(vm);
        }
    }
}