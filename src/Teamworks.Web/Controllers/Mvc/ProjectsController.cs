using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            string current = HttpContext.GetCurrentPersonId();

            RavenQueryStatistics stats;
            List<ProjectEntityCount.Result> results = DbSession
                .Query<ProjectEntityCount.Result, ProjectEntityCount>()
                .Statistics(out stats)
                .Customize(c =>
                           c.Include<ProjectEntityCount.Result>(r => r.People)
                               .Include<ProjectEntityCount.Result>(r => r.Project))
                .Where(r => r.People.Any(p => p == current))
                .ToList();

            if (results.Count == 0)
                return HttpNotFound();

            var vm = new ProjectsViewModel
                         {
                             CurrentPage = 1,
                             TotalCount = stats.TotalResults,
                             Projects = new List<ProjectsViewModel.Project>()
                         };

            foreach (ProjectEntityCount.Result result in results)
            {
                Person[] people = DbSession.Load<Person>(result.People);
                var project = DbSession.Load<Project>(result.Project);

                var p = result.MapTo<ProjectsViewModel.Project>();
                p.People = people.MapTo<PersonViewModel>();
                p.Description = project.Description;
                vm.Projects.Add(p);
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            string projectId = id.ToId("project");
            RavenQueryStatistics stats;
            IRavenQueryable<Activity> activities = DbSession
                .Query<Activity>()
                .Statistics(out stats)
                .Customize(c => c.Include<Activity>(r => r.Project)
                                    .Include<Activity>(r => r.People))
                .Where(r => r.Project == projectId);

            var project = DbSession.Include<Project>(p => p.People)
                .Load<Project>(projectId);

            if (project == null)
                return HttpNotFound();

            IRavenQueryable<Discussion> discussions = DbSession
                .Query<Discussion>()
                .Statistics(out stats)
                .Customize(c => c.Include<Discussion>(r => r.Entity))
                .Where(r => r.Entity == projectId);

            var vm = project.MapTo<ProjectViewModel>();
            vm.Activities = activities.MapTo<ProjectViewModel.Activity>();
            vm.Discussions = discussions.MapTo<ProjectViewModel.Discussion>();
            vm.People = DbSession.Load<Person>(project.People)
                .Where(p => p != null).MapTo<PersonViewModel>();

            return View(vm);
        }

        [HttpGet]
        public ActionResult Gantt(int id)
        {
            ViewBag.Endpoint = "api/projects/" + id;

            var project = DbSession
                .Load<Project>(id);

            var act = DbSession.Query
                <ActivitiesDuration.Result,
                    ActivitiesDuration>()
                .Where(a => a.Project == project.Id)
                .OrderBy(a => a.StartDate)
                .ToList()
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
                                 });

            ViewBag.ChartData = JsonConvert.SerializeObject(act);

            return View(project);
        }
    }
}