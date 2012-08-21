using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Raven.Client.Util;
using Teamworks.Core;
using Teamworks.Core.Extensions;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [SecureProject("projects/view/activities/view")]
    [RoutePrefix("projects/{projectId}/activities")]
    public class ActivitiesController : RavenController
    {
        [GET("{activityId}")]
        public ActionResult Details(int projectId, int activityId)
        {
            var list = DbSession.Query<Activity>()
                .Include(a => a.Project)
                .Include(a => a.People)
                .Where(r => r.Project == projectId.ToId("project")).ToList();

            var activity = list.FirstOrDefault(a => a.Id == activityId.ToId("activity"));

            if (activity == null)
                return HttpNotFound();

            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId.ToId("project"));

            if (project == null)
                return HttpNotFound();

            var vm = activity.MapTo<ActivityViewModelComplete>();
            vm.ProjectReference = project.MapTo<EntityViewModel>();
            vm.People = DbSession.Load<Person>(project.People)
                .Select(r => {
                        var result = r.MapTo<ActivityViewModelComplete.AssignedPersonViewModel>();
                        result.Assigned = r.Id.In(activity.People);
                        return result;
                    }).ToList();

            vm.TotalTimeLogged = activity.Timelogs.Sum(r => r.Duration);
            vm.Timelogs = activity.Timelogs.Select(r => {
                    var result = r.MapTo<TimelogViewModel>();
                    result.Person = DbSession.Load<Person>(r.Person).MapTo<EntityViewModel>();
                    return result;
                }).ToList();

            var discussions = activity.Discussions.Select(discussion => 
                DbSession.Load<Discussion>(discussion).MapTo<DiscussionViewModel>()).ToList();
            vm.Discussions = discussions;

            vm.Dependencies = list
                .Where(r => r.Id.ToIdentifier() != activityId)
                .Select(r => {
                    var result = r.MapTo<DependencyActivityViewModel>();
                    result.Dependency = r.Id.In(activity.Dependencies);
                    return result;
                })
                .ToList();
            return View(vm);
        }

        [POST("edit")]
        public ActionResult Update(int projectId, ActivityViewModel.Input model)
        {
            var activity = DbSession.Load<Activity>(model.Id);
            if (activity == null || activity.Project.ToIdentifier() == projectId)
                HttpNotFound();

            activity.Update(model.MapTo<Activity>(), DbSession);

            return new JsonNetResult { Data = activity.MapTo<ActivityViewModel>() };
        }

        [POST("")]
        public ActionResult Add(int projectId, ActivityViewModel model)
        {
            var project = DbSession
                .Load<Project>(projectId);

            var activity = Activity.Forge(project.Id.ToIdentifier(), model.Name, model.Description, model.Duration,
                                          model.StartDate);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
                {
                    Tags = {project.Id}
                });

            if (model.StartDate != DateTimeOffset.MinValue)
                activity.StartDate = model.StartDate;

            return new JsonNetResult { Data = activity.MapTo<ActivityViewModelComplete>() };
        }

        [POST("{activityId}/delete")]
        public ActionResult Remove(int projectId, int activityId)
        {
            var activity = DbSession
                .Query<Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == activityId.ToId("activity"));
            DbSession.Delete(activity);
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [POST("{activityId}/people")]
        public ActionResult AddPerson(int projectId, int activityId, string email)
        {
            var _projectId = projectId.ToId("project");
            var _activityId = activityId.ToId("activity");

            var person = DbSession.Query<Person>()
                .Customize(c => c.Include(_projectId))
                .Customize(c => c.Include(_activityId))
                .Where(p => p.Email == email).FirstOrDefault();

            if (person == null)
                return new HttpNotFoundResult();

            var activity = DbSession.Load<Activity>(activityId);
            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            activity.People.Add(person.Id);
            var result = person.MapTo<ActivityViewModelComplete.AssignedPersonViewModel>();
            result.Assigned = true;
            return new JsonNetResult { Data = result };
        }

        [POST("{activityId}/people/{personId}/delete")]
        public ActionResult RemovePerson(int projectId, int activityId, int personId)
        {
            var _projectId = projectId.ToId("project");
            var _activityId = activityId.ToId("activity");

            var person = DbSession
                .Include(_activityId)
                .Include(_projectId)
                .Load<Person>(personId);

            if (person == null)
                return new HttpNotFoundResult();

            var activity = DbSession.Load<Activity>(activityId);
            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            activity.People.Remove(person.Id);
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [AjaxOnly]
        [POST("{activityId}/discussions")]
        public ActionResult CreateDiscussion(int projectId, int activityId, DiscussionViewModel.Input model)
        {
            // todo error handling
            
            var activity = GetActivity(projectId, activityId);
            if (activity == null)
                return new HttpNotFoundResult();

            var person = DbSession.GetCurrentPerson();
            var discussion = Discussion.Forge(model.Name, model.Content, activity.Id, person.Id);
            DbSession.Store(discussion);
            activity.Discussions.Add(discussion.Id);

            var vm = discussion.MapTo<DiscussionViewModel>();
            vm.Person = person.MapTo<PersonViewModel>();
            vm.Entity = activity.MapTo<EntityViewModel>();
            return new JsonNetResult() { Data = vm };
        }

        [NonAction]
        public Activity GetActivity(int projectId, int activityId)
        {
            var activity = DbSession.Load<Activity>(activityId);
            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return null;
            return activity;
        }

        public class MyClass<T> : ObjectReferenceEqualityComparerer<T> where T : Entity
        {

            public override bool Equals(T a, T b)
            {
                return a.Id == b.Id;
            }
        }
    }
}