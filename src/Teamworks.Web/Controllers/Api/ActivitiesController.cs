using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Activity = Teamworks.Web.Models.Api.Activity;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor]
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectId}/activities")]
    public class ActivitiesController : RavenApiController
    {
        public ActivitiesController() {}
        public ActivitiesController(IDocumentSession session)
            : base(session) {}

        public IEnumerable<Activity> Get(int projectId)
        {
            var activities = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .Where(a => a.Project == projectId.ToId("project"));

            return Mapper.Map<IEnumerable<Core.Activity>,
                IEnumerable<Activity>>(activities.ToList());
        }

        public Activity Get(int id, int projectId)
        {
            var activity = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == id.ToId("activity"));

            Request.ThrowNotFoundIfNull(activity);
            return Mapper.Map<Core.Activity, Activity>(activity);
        }

        public HttpResponseMessage Post(int projectId, Activity model)
        {
            var project = DbSession
                .Load<Core.Project>(projectId);

            var activity = Core.Activity.Forge(project.Id, model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
                                                        {
                                                            Tags = {project.Id}
                                                        });

            // calculate dependency graph 
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;

            var domain = DbSession.Query<Core.Activity>()
                .Where(a => a.Project == project.Id).ToList();

            activity.StartDate = project.StartDate.AddMinutes(GetAccumulatedDuration(domain, activity));

            var activities = Mapper.Map<Core.Activity, Activity>(activity);

            // todo add header of location

            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        [PUT("")]
        public HttpResponseMessage Put(int projectId,
                                       Activity model)
        {
            var activity = DbSession
                .Load<Core.Activity>(model.Id);

            Request.ThrowNotFoundIfNull(activity);

            activity.Name = model.Name ?? activity.Name;
            activity.Description = model.Description ?? activity.Description;
            if (activity.Duration != model.Duration)
            {
                var domain = DbSession.Query<Core.Activity>()
                    .Where(a => a.Project == projectId.ToId("project")).ToList();
                OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }

            activity.Duration = model.Duration;
            var activities = Mapper.Map<Core.Activity, Activity>(activity);
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        public HttpResponseMessage Delete(int id, int projectId)
        {
            var activity = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == id.ToId("activity"));

            Request.ThrowNotFoundIfNull(activity);

            DbSession.Delete(activity);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private double GetAccumulatedDuration(List<Core.Activity> domain, Core.Activity activity, int duration = 0)
        {
            var parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;

            return parent.Dependencies.Count == 0
                       ? parent.Duration
                       : GetAccumulatedDuration(domain, parent, parent.Duration);
        }

        private void OffsetDuration(List<Core.Activity> domain, Core.Activity parent, int offset)
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

        #region Precedence

        [GET("{id}/precedences")]
        public IEnumerable<ActivityRelation> GetPre(int id, int projectId)
        {
            var activity = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Id == id.ToId("activity")
                                     && a.Project == projectId.ToId("project"));

            Request.ThrowNotFoundIfNull(activity);
            return activity.DependencyGraph();
            ;
        }

        [POST("{id}/precedences/{precedenceid}")]
        public HttpResponseMessage PostPre(int id, int projectId,
                                           int precedenceid)
        {
            /*
            var project = DbSession
                .Load<Core.Project>(projectId);
            var activity = DbSession.Load<Core.Activity>(id);

            if (project == null || activity == null || !project.Activities.Any(t => t.ToIdentifier() == id))
                Request.ThrowNotFoundIfNull();

            var depid = "activities/" + precedenceid;
            if (activity.Dependencies.Contains(depid))
                Request.CreateResponse(HttpStatusCode.NotModified); //todo is valid response code?

            activity.Dependencies.Add(depid);
            return Request.CreateResponse(HttpStatusCode.Created);
            */
            return null;
        }

        [DELETE("{id}/precedences/{precedenceid}")]
        public HttpResponseMessage Delete(int id, int projectId, int precedenceid)
        {
            /*
            var project = DbSession
                .Load<Core.Project>(projectId);
            var activity = DbSession.Load<Core.Activity>(id);

            if (project == null || activity == null || !project.Activities.Any(t => t.ToIdentifier() == id))
                Request.ThrowNotFoundIfNull();

            var depid = "activities/" + precedenceid;
            if (!activity.Dependencies.Contains(depid))
                Request.CreateResponse(HttpStatusCode.NotModified); //todo is valid response code?

            activity.Dependencies.Remove(depid);
            return Request.CreateResponse(HttpStatusCode.Created);
        
             */
            return null;
        }

        #endregion

        #region People

        #endregion
    }
}