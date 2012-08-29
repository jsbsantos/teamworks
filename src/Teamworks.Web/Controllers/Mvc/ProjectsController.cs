﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Extensions;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("projects")]
    public class ProjectsController : AppController
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
            return View("View", vm);
        }

        [GET("{projectId}")]
        [SecureProject("projects/view")]
        public ActionResult Details(int projectId)
        {
            var id = projectId.ToId("project");
            var project = DbSession.Load<Project>(projectId);

            if (project == null)
                return HttpNotFound();

            RavenQueryStatistics stats;
            var activities = DbSession
                .Query<Activity>()
                .Statistics(out stats)
                .Where(r => r.Project == id);
            var discussions = DbSession
                .Query<Discussion>()
                .Statistics(out stats)
                .Where(r => r.Entity == id);

            var vm = project.MapTo<ProjectViewModel>();
            vm.Activities = activities.MapTo<ProjectViewModel.Activity>();
            vm.Discussions = discussions.MapTo<ProjectViewModel.Discussion>();
            vm.People = DbSession.Load<Person>(project.People)
                .Where(p => p != null).MapTo<PersonViewModel>();

            foreach (var activity in activities)
            {
                vm.Timelogs = vm.Timelogs
                    .Concat(activity.Timelogs.MapTo<ProjectViewModel.Timelog>()
                                .Select(a =>
                                {
                                    a.Activity = activity.MapTo<EntityViewModel>();
                                    a.Project = project.MapTo<EntityViewModel>();
                                    a.Person.Name = vm.People.Single(p => p.Id == a.Person.Id).Name;
                                    return a;
                                }))
                    .ToList();
            }

            vm.Timelogs = vm.Timelogs
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.Activity.Id).ToList();

            return View(vm);
        }

        [POST("")]
        public ActionResult Post(ProjectsViewModel.Input model)
        {
            if (!ModelState.IsValid)
                return View("View");

            var person = DbSession.GetCurrentPerson();

            var project = Project.Forge(model.Name, model.Description);
            DbSession.Store(project);
            project.Grant(string.Empty, person);
            project.Initialize(DbSession);

            var projectViewModel = project.MapTo<ProjectsViewModel.Project>();
            projectViewModel.People = new List<PersonViewModel> {person.MapTo<PersonViewModel>()};

            if (Request.IsAjaxRequest())
                return new JsonNetResult {Data = projectViewModel};

            return RedirectToRoute("projects_get");
        }
        [POST("edit")]
        public ActionResult Put(ProjectsViewModel.Input model)
        {
            if (!ModelState.IsValid)
                return View("View");

            var project = DbSession.Load<Project>(model.Id);
            project.Name = model.Name;
            project.Description = model.Description;

            return new JsonNetResult { Data = model };
        }

        [POST("{projectId}")]
        [SecureProject("projects/delete")]
        public ActionResult Delete(int projectId)
        {
            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return HttpNotFound();

            project.Delete(DbSession);

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);

            return View("View");
        }

        [AjaxOnly]
        [SecureProject("projects/view")]
        [POST("{projectId}/people/add")]
        public ActionResult AddPerson(int projectId, string email)
        {
            var id = projectId.ToId("project");

            var person = DbSession.GetPersonByEmail(email);

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();

            project.Grant(string.Empty, person);
            return new JsonNetResult {Data = person.MapTo<PersonViewModel>()};
        }

        [AjaxOnly]
        [SecureProject("projects/view")]
        [POST("{projectId}/people/remove/{personId}")]
        public ActionResult RemovePerson(int projectId, int personId)
        {
            var person = DbSession.Load<Person>(personId);

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();

            project.Revoke(string.Empty, person);
            return new JsonNetResult {Data = person.MapTo<PersonViewModel>()};
        }

        [GET("{projectId}/gantt")]
        public ActionResult Gantt(int projectId)
        {
            ViewBag.Endpoint = "api/projects/" + projectId;

            DbSession.SecureFor(DbSession.GetCurrentPersonId(), "god");

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

        [NonAction]
        public override Breadcrumb[] CreateBreadcrumb()
        {
            var breadcrumb = new List<Breadcrumb>();
            if (RouteData.Values.ContainsKey("projectId"))
            {
                var projectId = int.Parse(RouteData.Values["projectId"].ToString());
                var project = DbSession.Load<Project>(projectId);

                breadcrumb.Add(
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_get"),
                            Name = "Projects"
                        });
                breadcrumb.Add(
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_get", new {projectId}),
                            Name = project.Name
                        }
                    );
            }
            return breadcrumb.ToArray();
        }
    }
}