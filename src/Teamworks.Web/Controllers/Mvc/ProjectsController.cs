using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            var current = HttpContext.GetCurrentPersonId();
            
            RavenQueryStatistics stats;
            var results = DbSession
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

            foreach (var result in results)
            {
                var people = DbSession.Load<Person>(result.People);
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
            var projectId = id.ToId("project");
            RavenQueryStatistics stats;
            var activities = DbSession
                .Query<Activity>()
                .Statistics(out stats)
                .Customize(c => c.Include<Activity>(r => r.Project)
                                    .Include<Activity>(r => r.People))
                .Where(r => r.Project == projectId);

            var project = DbSession.Include<Project>(p => p.People)
                .Load<Project>(projectId);

            if (project == null)
                return HttpNotFound();

            var discussions = DbSession
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
    }
}