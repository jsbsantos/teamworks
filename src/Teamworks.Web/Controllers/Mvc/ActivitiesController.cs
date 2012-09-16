using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
    public class ActivitiesController : AppController
    {
        [GET("{activityId}/discussions")]
        public ActionResult BreadcrumbRedirect(int projectId, int activityId)
        {
            return RedirectToAction("Details", new { projectId, activityId });
        }
        
        [GET("{activityId}")]
        public ActionResult Details(int projectId, int activityId)
        {
            var list = DbSession.Query<Activity>()
                .Include(a => a.Project)
                .Include(a => a.People)
                .Where(r => r.Project == projectId.ToId("project"))
                .OrderBy(a => a.Id).ToList();

            var activity = list.FirstOrDefault(a => a.Id == activityId.ToId("activity"));

            if (activity == null)
                return HttpNotFound();

            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId.ToId("project"));

            if (project == null)
                return HttpNotFound();

            var vm = activity.MapTo<ActivityViewModelComplete>();
            vm.Project = project.MapTo<EntityViewModel>();
            vm.People = DbSession.Load<Person>(project.People)
                .Select(r =>
                    {
                        var result = r.MapTo<ActivityViewModelComplete.AssignedPersonViewModel>();
                        result.Assigned = r.Id.In(activity.People);
                        return result;
                    }).ToList();

            vm.TotalTimeLogged = activity.Timelogs.Sum(r => r.Duration);
            vm.Timelogs = activity.Timelogs.Select(r =>
                {
                    var result = r.MapTo<TimelogViewModel>();
                    result.Person = DbSession.Load<Person>(r.Person).MapTo<EntityViewModel>();
                    return result;
                }).ToList();

            var discussions = DbSession.Query<Discussion>()
                .Where(d => d.Entity == activity.Id)
                .OrderBy(d => d.Id)
                .MapTo<DiscussionViewModel>()
                .ToList();

            vm.Discussions = discussions;

            vm.Dependencies = list
                .Where(r => r.Id.ToIdentifier() != activityId)
                .Select(r =>
                    {
                        var result = r.MapTo<DependencyActivityViewModel>();
                        result.Dependency = r.Id.In(activity.Dependencies);
                        return result;
                    })
                .ToList();

            vm.Todos = activity.Todos.OrderBy(t => t.Id)
                .Select(r =>
                    {
                        var result = r.MapTo<TodoViewModel.Output>();
                        result.Person = r.Person != null ? DbSession.Load<Person>(r.Person).MapTo<EntityViewModel>() : null;
                        return result;
                    });

            vm.Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", DbSession.GetCurrentPersonId(), activity.Id)));
            return View(vm);
        }

        [POST("{activityId}/edit")]
        public ActionResult Update(int projectId, int activityId, ActivityViewModel.Input model)
        {
            var activity = DbSession.Load<Activity>(activityId);
            if (activity == null || activity.Project.ToIdentifier() == projectId)
                HttpNotFound();
            model.Dependencies = model.Dependencies ?? new List<int>();
            activity.Update(model.MapTo<Activity>(), DbSession);
            var data = activity.MapTo<ActivityViewModel>();

            data.Project = DbSession.Load<Project>(model.Project).MapTo<EntityViewModel>();
            return new JsonNetResult {Data = data};
        }

        [POST("")]
        public ActionResult Add(int projectId, ActivityViewModel model)
        {
            var project = DbSession
                .Load<Project>(projectId);

            var activity = Activity.Forge(project.Id.ToIdentifier(), model.Name, model.Description, model.Duration,
                                          model.StartDate == DateTimeOffset.MinValue
                                              ? project.StartDate
                                              : model.StartDate);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
                {
                    Tags = {project.Id}
                });

            if (model.StartDate != DateTimeOffset.MinValue)
                activity.StartDateConsecutive = activity.StartDate = model.StartDate;

            return new JsonNetResult {Data = activity.MapTo<ActivityViewModelComplete>()};
        }

        [POST("{activityId}/delete")]
        public ActionResult Delete(int projectId, int activityId)
        {
            var activity = DbSession
                .Query<Activity>()
                .Where(a => a.Project == projectId.ToId("project")
                                     && a.Id == activityId.ToId("activity"))
                .FirstOrDefault();
            if (activity != null)
                activity.Delete(DbSession);
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
            return new JsonNetResult {Data = result};
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

            var vm = discussion.MapTo<DiscussionViewModel>();
            vm.Person = person.MapTo<PersonViewModel>();
            vm.Entity = activity.MapTo<EntityViewModel>();
            return new JsonNetResult {Data = vm};
        }

        [AjaxOnly]
        [POST("{activityId}/todos")]
        public ActionResult AddTodo(int projectId, int activityId, TodoViewModel model)
        {
            var activity = GetActivity(projectId, activityId);
            if (activity == null)
                return new HttpNotFoundResult();

            var todo = Todo.Forge(model.Name, model.Description, model.DueDate);
            todo.Id = activity.GenerateNewTodoId();
            activity.Todos.Add(todo);

            var vm = todo.MapTo<TodoViewModel.Output>();
            vm.Person = null;
            return new JsonNetResult { Data = vm };
        }

        [AjaxOnly]
        [POST("{activityId}/todos/{todoid}")]
        public ActionResult ToggleTodo(int projectId, int activityId, int todoid, bool state)
        {
            var activity = GetActivity(projectId, activityId);
            if (activity == null)
                return new HttpNotFoundResult();

            var todo = activity.Todos.Where(t => t.Id == todoid).FirstOrDefault();
            if (todo == null)
                return new HttpNotFoundResult();

            var person = DbSession.GetCurrentPerson();
            todo.Completed = state;
            todo.Person = person.Id;

            var vm = todo.MapTo<TodoViewModel.Output>();
            vm.Person = person.MapTo<EntityViewModel>();
            return new JsonNetResult { Data = vm };
        }

        [NonAction]
        public Activity GetActivity(int projectId, int activityId)
        {
            var activity = DbSession.Load<Activity>(activityId);
            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return null;
            return activity;
        }

        [NonAction]
        public override Breadcrumb[] CreateBreadcrumb()
        {
            var projectId = int.Parse(RouteData.Values["projectId"].ToString());
            var activityId = int.Parse(RouteData.Values["activityId"].ToString());

            var project = DbSession.Load<Project>(projectId);
            var activity = DbSession.Load<Activity>(activityId);

            var breadcrumb = new List<Breadcrumb>
                {
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_get"),
                            Name = "Projects"
                        },
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("projects_details", new {projectId}),
                            Name = project.Name
                        },
                    new Breadcrumb
                        {
                            Url =
                                Url.RouteUrl("activities_details", new {projectId, activityId = UrlParameter.Optional}),
                            Name = "Activities"
                        },
                    new Breadcrumb
                        {
                            Url = Url.RouteUrl("activities_details", new {projectId, activityId}),
                            Name = activity.Name
                        }
                };

            return breadcrumb.ToArray();
        }
    }
}