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

        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int projectId)
        {
            return View();
        }

        [HttpGet]
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
                var result = r.MapTo<ActivityViewModel.TimelogViewModel>();
                //result.Person = DbSession.Load<Person>(r.Person).MapTo<EntityViewModel>();
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

        /*
        [HttpPost]
        public ActionResult Precedences(int projectId, int activityId, int[] precedences)
        {
            return ActivityServices.SetPrecedence(activityId, projectId, precedences) ?
                new HttpStatusCodeResult(HttpStatusCode.Created) :
                HttpNotFound();
        }
        */

        [HttpPost]
        public ActionResult Update(int projectId, int activityId, ActivityViewModel model)
        {
            var activity = ActivityServices.Update(model.MapTo<Activity>());
            if (activity == null)
                HttpNotFound();

            Response.StatusCode = (int)HttpStatusCode.Created;
            return new JsonResult()
            {
                Data = activity.MapTo<ActivityViewModel>(),
                ContentEncoding = System.Text.Encoding.UTF8
            };

        }
        
        [AjaxOnly]
        [POST("projects/{id}/activities")]
        public ActionResult Add(int id, ActivityViewModel.Input model)
        {
            var project = DbSession
                 .Load<Project>(id);

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
    }
}