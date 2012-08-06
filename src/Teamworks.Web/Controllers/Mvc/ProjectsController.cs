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
            // DbSession.SecureFor(current, Global.Constants.Operation);

            RavenQueryStatistics stats;
            var results = DbSession
                .Query<ProjectEntityCount.Result, ProjectEntityCount>()
                .Statistics(out stats)
                .Customize(c =>
                           c.Include<ProjectEntityCount.Result>(r => r.People)
                               .Include<ProjectEntityCount.Result>(r => r.Project))
                .Where(r => r.People.Any(p => p == current))
                .ToList();

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
            RavenQueryStatistics stats;
            var results = DbSession
                .Query<ProjectsEntitiesRelated.Result, ProjectsEntitiesRelated>()
                .Statistics(out stats)
                .Customize(c =>
                           c.Include<ProjectsEntitiesRelated.Result>(r => r.Entity)
                               .Include<ProjectsEntitiesRelated.Result>(r => r.Project))
                .Where(r => r.Project == id.ToId("project"));

            var project = DbSession.Load<Project>(id.ToId("project"));
            return View(new ProjectViewModel
                            {
                                Summary = project.MapTo<ProjectViewModel.ProjectSummary>(),
                                People = DbSession.Load<Person>(project.People).MapTo<PersonViewModel>(),
                                Activities = results.OfType<Activity>().As<Activity>().MapTo<ProjectViewModel.Activity>(),
                                Discussions = results.OfType<Discussion>().As<Discussion>().MapTo<ProjectViewModel.Discussion>()
                            });
        }
    }
}