using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;
using Teamworks.Web.Views;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("projects")]
    public class ProjectsController : RavenController
    {
        [GET("")]
        [Secure("projects/view")]
        public ActionResult Get(int page = 1)
        {
            RavenQueryStatistics stats;
            var projects = DbSession.Query<Project>()
                .Statistics(out stats)
                .Customize(c => c.Include<Project>(p => p.People))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            var vm = new ProjectsViewModel
                         {
                             CurrentPage = page,
                             TotalCount = stats.TotalResults,
                             Projects = new List<ProjectsViewModel.Project>()
                         };

            if (projects.Count > 0)
            {
                var results = DbSession
                    .Query<ProjectEntityCount.Result, ProjectEntityCount>()
                    .Where(r => r.Project.In(projects.Select(p => p.Id)))
                    .ToList();

                foreach (var result in results)
                {
                    var project = DbSession.Load<Project>(result.Project);

                    var projectViewModel = result.MapTo<ProjectsViewModel.Project>();
                    projectViewModel.People = DbSession.Load<Person>(project.People)
                        .Where(p => p != null).MapTo<PersonViewModel>();

                    projectViewModel.Name = project.Name;
                    projectViewModel.Description = project.Description;
                    vm.Projects.Add(projectViewModel);
                }
            }
            return View("Index", vm);
        }

        [POST("")]
        public ActionResult Post(ProjectsViewModel.Input model)
        {
            if (ModelState.IsValid)
            {
                var project = Project.Forge(model.Name, model.Description);
                DbSession.Store(project);

                var current = HttpContext.GetCurrentPerson();
                project.People.Add(current.Id);

                var projectViewModel = project.MapTo<ProjectsViewModel.Project>();
                projectViewModel.People = new List<PersonViewModel> {current.MapTo<PersonViewModel>()};

                if (Request.IsAjaxRequest())
                    return new JsonNetResult
                               {
                                   Data = projectViewModel
                               };
                return RedirectToRoute("projects_get");
            }
            return View("Index");
        }

        [GET("{projectId}")]
        [SecureProject("projects/view")]
        public ActionResult Details(int projectId)
        {
            var id = projectId.ToId("project");

            RavenQueryStatistics stats;
            var activities = DbSession
                .Query<Activity>()
                .Statistics(out stats)
                .Customize(c => c.Include<Activity>(r => r.Project)
                                    .Include<Activity>(r => r.People))
                .Where(r => r.Project == id);

            var project = DbSession.Include<Project>(p => p.People)
                .Load<Project>(id);

            if (project == null)
                return HttpNotFound();

            var discussions = DbSession
                .Query<Discussion>()
                .Statistics(out stats)
                .Customize(c => c.Include<Discussion>(r => r.Entity))
                .Where(r => r.Entity == id);

            var vm = project.MapTo<ProjectViewModel>();
            vm.Activities = activities.MapTo<ProjectViewModel.Activity>();
            vm.Discussions = discussions.MapTo<ProjectViewModel.Discussion>();
            vm.People = DbSession.Load<Person>(project.People)
                .Where(p => p != null).MapTo<PersonViewModel>();

            return View(vm);
        }

        [AjaxOnly]
        [POST("{projectId}/people/{personIdOrEmail}")]
        public ActionResult AddPerson(int projectId, string personIdOrEmail)
        {
            var id = projectId.ToId("project");

            var person = DbSession.Query<Person>()
                .Customize(c => c.Include(id))
                .Where(p => p.Email == personIdOrEmail).FirstOrDefault();

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();

            return new ContentResult
                       {
                           Content = Utils.ToJson(person.MapTo<PersonViewModel>())
                       };
        }

        [AjaxOnly]
        [DELETE("{projectId}/people/{personIdOrEmail}")]
        public ActionResult RemovePerson(int projectId, string personIdOrEmail)
        {
            var person = DbSession.Query<Person>()
                .Customize(c => c.Include(projectId.ToId("project")))
                .Where(p => p.Email == personIdOrEmail).FirstOrDefault();

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();

            return new ContentResult
                       {
                           Content = Utils.ToJson(person.MapTo<PersonViewModel>())
                       };
        }

        public ActionResult Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            return new EmptyResult();
        }

        [GET("{projectId}/gantt")]
        public ActionResult Gantt(int projectId)
        {
            ViewBag.Endpoint = "api/projects/" + projectId;

            DbSession.SecureFor(ControllerContext.HttpContext.GetCurrentPersonId(), "GOD");

            var project = DbSession
                .Load<Project>(projectId);

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