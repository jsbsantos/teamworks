using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Business;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ActivitiesController : RavenController
    {
        private ActivityServices ActivityServices { get; set; }
        public ActivitiesController()
        {
            ActivityServices = new Lazy<ActivityServices>(() => new ActivityServices { DbSession = DbSession }).Value;
        }

        [GET("projects/{projectId}/activities/{activityId}")]
        public ActionResult Details(int projectId, int activityId)
        {
            var list = DbSession.Query<Activity>()
                .Include(a => a.Project)
                .Include(a => a.People)
                .Where(r => r.Project == projectId.ToId("project")).ToList();

            var activity = list.FirstOrDefault(a => a.Id == activityId.ToId("activity"));

            if (activity == null)
                return HttpNotFound();

            var project = DbSession.Load<Project>(projectId.ToId("project"));

            if (project == null)
                return HttpNotFound();

            var vm = activity.MapTo<ActivityViewModelComplete>();

            vm.ProjectReference = project.MapTo<EntityViewModel>();

            vm.AssignedPeople =
                DbSession.Load<Person>(activity.People.Distinct()).Select(
                    r => r.MapTo<PersonViewModel>()).ToList();

            vm.TotalTimeLogged = activity.Timelogs.Sum(r => r.Duration);
            vm.Timelogs = activity.Timelogs.Select(r =>
            {
                var result = r.MapTo<TimelogViewModel>();
                return result;
            }).ToList();
            ViewBag.Results = vm;

            vm.Dependencies = list.Select(r =>
            {
                var result = r.MapTo<DependencyActivityViewModel>();
                result.Dependency = r.Id.In(activity.Dependencies);
                return result;
            })
                .ToList();
            return View(vm);
        }

        [PUT("projects/{projectId}/activities")]
        public ActionResult Update(int projectId, ActivityViewModel model)
        {
            var activity = ActivityServices.Update(model.MapTo<Activity>());
            if (activity == null)
                HttpNotFound();

            Response.StatusCode = (int)HttpStatusCode.Created;
            return new JsonResult()
            {
                Data = activity.MapTo<ActivityViewModel>()
            };
        }
        
        [POST("projects/{projectId}/activities")]
        public ActionResult Add(int projectId, ActivityViewModel.Input model)
        {
            var project = DbSession
                 .Load<Project>(projectId);

            var activity = Activity.Forge(project.Id.ToIdentifier(), model.Name, model.Description, model.Duration, model.StartDate);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
            {
                Tags = { project.Id }
            });
            
            if(model.StartDate != DateTimeOffset.MinValue)
                activity.StartDate = model.StartDate;

            return new ContentResult
            {
                Content = Utils.ToJson(activity.MapTo<ActivityViewModelComplete>())
            };
        }

        [DELETE("projects/{projectId}/activities/{activityId}")]
        public ActionResult Remove(int projectId, int activityId)
        {
            var activity = DbSession
                .Query<Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == activityId.ToId("activity"));
            DbSession.Delete(activity);
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [POST("projects/{projectId}/activities/{activityId}/people")]
        public ActionResult AddPerson(int projectId, int activityId, string personIdOrEmail)
        {
            var _projectId = projectId.ToId("project");
            var _activityId = activityId.ToId("activity");

            var person = DbSession.Query<Person>()
                .Customize(c => c.Include(_projectId))
                .Customize(c => c.Include(_activityId))
                .Where(p => p.Email == personIdOrEmail || p.Id == personIdOrEmail).FirstOrDefault();

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();
            
            var activity = DbSession.Load<Project>(projectId);
            if (activity == null)
                return new HttpNotFoundResult();
            
            activity.People.Add(person.Id);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        } 

        [DELETE("projects/{projectId}/activities/{activityId}/people")]
        public ActionResult RemovePerson(int projectId, int activityId, string personIdOrEmail)
        {
            var _projectId = projectId.ToId("project");
            var _activityId = activityId.ToId("activity");

            var person = DbSession.Query<Person>()
                .Customize(c => c.Include(_projectId))
                .Customize(c => c.Include(_activityId))
                .Where(p => p.Email == personIdOrEmail || p.Id == personIdOrEmail).FirstOrDefault();

            if (person == null)
                return new HttpNotFoundResult();

            var project = DbSession.Load<Project>(projectId);
            if (project == null)
                return new HttpNotFoundResult();

            var activity = DbSession.Load<Project>(projectId);
            if (activity == null)
                return new HttpNotFoundResult();

            activity.People.Remove(person.Id);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        } 
    }
}