using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Api;
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
            if (activity == null || activity.Project.ToIdentifier() == projectId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return activity.MapTo<ActivityViewModel>();
        }

        public HttpResponseMessage Post(int projectId, ActivityViewModel model)
        {
            var project = DbSession.Load<Project>(projectId);
            var activity = Activity.Forge(projectId, model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;

            var domain = DbSession.Query<Activity>()
                .Where(a => a.Project == project.Id).ToList();

            activity.StartDate = project.StartDate
                .AddMinutes(Activity.GetAccumulatedDuration(domain, activity));

            var activities = activity.MapTo<ActivityViewModel>();
            
            // todo add header of location
            
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        [PUT("")]
        public HttpResponseMessage Put(int projectId,
                                       ActivityViewModel model)
        {
            var activity = DbSession
                .Load<Activity>(model.Id);

            Request.NotFound(activity);

            activity.Name = model.Name ?? activity.Name;
            activity.Description = model.Description ?? activity.Description;
            if (activity.Duration != model.Duration)
            {
                var domain = DbSession.Query<Activity>()
                    .Where(a => a.Project == projectId.ToId("project")).ToList();
                OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }

            activity.Duration = model.Duration;
            var activities = activity.MapTo<ActivityViewModel>();
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        public HttpResponseMessage Delete(int id, int projectId)
        {
            var activity = DbSession.Load<Activity>(id);
            if (activity != null && activity.Project.ToIdentifier() != projectId)
                DbSession.Delete(activity);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Precedence
        /*
        [POST("{id}/precedences")]
        public HttpResponseMessage PostPre(int id, int projectId,
                                           int[] precedences)
        {
            if (ActivityServices.AddPrecedence(id, projectId, precedences) == null)
                Request.ThrowNotFound();
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [DELETE("{id}/precedences")]
        public HttpResponseMessage Delete(int id, int projectId, int[] precedences)
        {
            if (!ActivityServices.RemovePrecedence(id, projectId, precedences))
                Request.ThrowNotFound();
            return Request.CreateResponse(HttpStatusCode.Created);
        }
        
        */ 
        #region Private

        private void OffsetDuration(ICollection<Activity> domain, Activity parent, int offset)
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

        #endregion
    }
}