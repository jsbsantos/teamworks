using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Extensions;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [SecureProject("projects/view")]
    [RoutePrefix("api/projects/{projectId}/activities")]
    public class ActivitiesController : RavenApiController
    {
        #region General

        public IEnumerable<ActivityViewModel> Get(int projectId)
        {
            var id = projectId.ToId("project");
            var activities = DbSession.Query<Activity>()
                .Where(a => a.Project == id);

            return activities.MapTo<ActivityViewModel>();
        }

        public ActivityViewModel GetById(int id, int projectId)
        {
            var activity = DbSession.Load<Activity>(id);
            if (activity == null || activity.Project.ToIdentifier() != projectId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return activity.MapTo<ActivityViewModel>();
        }

        public HttpResponseMessage Post(int projectId, CompleteActivityViewModel model)
        {
            var project = DbSession.Load<Project>(projectId);
            var activity = Activity.Forge(projectId, model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies.Select(s => s.ToId("activity")).ToList();

            var domain = DbSession.Query<Activity>()
                .Where(a => a.Project == project.Id).ToList();

            activity.StartDate = project.StartDate
                .AddMinutes(Activity.GetAccumulatedDuration(domain, activity));

            var value = activity.MapTo<ActivityViewModel>();
            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            var uri = Url.Link("api_activities_getbyid", new {projectId, id = activity.Id.ToIdentifier()});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [PUT("")]
        public HttpResponseMessage Put(int projectId,
                                       CompleteActivityViewModel model)
        {
            var activity = DbSession
                .Load<Activity>(model.Id);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            activity.Update(model.MapTo<Activity>(), DbSession);

            var activities = activity.MapTo<ActivityViewModel>();
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        public HttpResponseMessage Delete(int id, int projectId)
        {
            var activity = DbSession.Load<Activity>(id);
            if (activity != null && activity.Project.ToIdentifier() == projectId)
                DbSession.Delete(activity);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Precedence

        [GET("{id}/precedences")]
        public IEnumerable<ActivityViewModel> GetPrecedence(int id, int projectId)
        {
            var pid = projectId.ToId("project");
            var aid = id.ToId("activity");

            var activity = DbSession.Query<Activity>()
                .Where(a => a.Project == pid && a.Id == aid).FirstOrDefault();

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return activity.Dependencies.MapTo<ActivityViewModel>();
        }

        [POST("{id}/precedences")]
        public HttpResponseMessage PostPrecedence(int id, int projectId, IEnumerable<int> precedences)
        {
            var pid = projectId.ToId("project");
            var aid = projectId.ToId("activity");

            var activities = DbSession.Query<Activity>()
                .Where(a => a.Project == pid).ToList();

            var activity = activities.Where(a => a.Id == aid).FirstOrDefault();
            if (activity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            activity.Dependencies = activities.Select(a => a.Id)
                .Intersect(precedences.Select(d => d.ToId("activity")))
                .ToList();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [DELETE("{id}/precedences")]
        public HttpResponseMessage Delete(int id, int projectId, IEnumerable<int> precedences)
        {
            var pid = projectId.ToId("project");
            var aid = projectId.ToId("activity");

            var activity = DbSession.Query<Activity>()
                .Where(a => a.Project == pid && a.Id == aid).FirstOrDefault();

            if (activity != null && activity.Project.ToIdentifier() == projectId)
                activity.Dependencies = activity.Dependencies
                    .Except(precedences.Select(d => d.ToId("activity")))
                    .ToList();

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #region Private

        [NonAction]
        public void OffsetDuration(ICollection<Activity> domain, Activity parent, int offset)
        {
            var children = domain.Where(a => a.Dependencies.Contains(parent.Id)).ToList();
            foreach (var child in children)
            {
                child.StartDate = child.StartDate.AddMinutes(offset);
                child.Name += offset;
                domain.Remove(child);
                OffsetDuration(domain, child, offset);
            }
        }

        #endregion

        #endregion

        #region People

        [GET("{id}/assignees")]
        public IEnumerable<PersonViewModel> GetAssignees(int id, int projectId)
        {
            var pid = projectId.ToId("project");
            var aid = projectId.ToId("activity");

            var activity = DbSession.Query<Activity>()
                .Where(a => a.Project == pid && a.Id == aid).FirstOrDefault();

            var people = DbSession.Load<Person>(activity.People);
            return people.MapTo<PersonViewModel>();
        }

        //todo change response?
        [POST("{id}/assignees")]
        public HttpResponseMessage PostAssignees(int id, int projectId, IEnumerable<int> ids)
        {
            var pid = projectId.ToId("project");
            var aid = projectId.ToId("activity");

            var activity = DbSession.Query<Activity>()
                .Customize(c => c.Include(projectId.ToId("project")))
                .Where(a => a.Project == pid && a.Id == aid).FirstOrDefault();

            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId);

            var people = DbSession.Query<Person>()
                .Where(p => p.Id.In(ids.Select(i => i.ToId("person"))));

            activity.People = activity.People.Union(people.Select(p => p.Id).Intersect(project.People)).ToList();

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [DELETE("{id}/assignees")]
        public HttpResponseMessage DeleteAssignees(int id, int projectId, IEnumerable<int> ids)
        {
            var pid = projectId.ToId("project");
            var aid = projectId.ToId("activity");

            var activity = DbSession.Query<Activity>()
                .Customize(c => c.Include(projectId.ToId("project")))
                .Where(a => a.Project == pid && a.Id == aid).FirstOrDefault();

            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId);

            var people = DbSession.Query<Person>()
                .Where(p => p.Id.In(ids.Select(i => i.ToId("person"))));

            activity.People = activity.People.Except(people.Select(p => p.Id).Intersect(project.People)).ToList();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #endregion
    }
}