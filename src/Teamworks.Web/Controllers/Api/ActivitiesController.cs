using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Business;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Extensions.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [SecureProject("projects/view")]
    [RoutePrefix("api/projects/{projectId}/activities")]
    public class ActivitiesController : RavenApiController
    {
        #region General

        private ActivityServices ActivityServices { get; set; }

        public ActivitiesController()
        {
            ActivityServices = new Lazy<ActivityServices>(() => new ActivityServices() { DbSession = DbSession }).Value;
        }

        public IEnumerable<Activity> Get(int projectId)
        {
            IRavenQueryable<Core.Activity> activities = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .Where(a => a.Project == projectId.ToId("project"));

            return Mapper.Map<IEnumerable<Core.Activity>,
                IEnumerable<Activity>>(activities.ToList());
        }

        public Activity Get(int id, int projectId)
        {
            Core.Activity activity = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == id.ToId("activity"));

            Request.NotFound(activity);
            return Mapper.Map<Core.Activity, Activity>(activity);
        }

        public HttpResponseMessage Post(int projectId, Activity model)
        {
            var project = DbSession
                .Load<Project>(projectId);

            Core.Activity activity = Core.Activity.Forge(project.Id.ToIdentifier(), model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
            {
                Tags = { project.Id }
            });

            // calculate dependency graph 
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;

            List<Core.Activity> domain = DbSession.Query<Core.Activity>()
                .Where(a => a.Project == project.Id).ToList();

            activity.StartDate = project.StartDate.AddMinutes(ActivityServices.GetAccumulatedDuration(domain, activity));

            Activity activities = Mapper.Map<Core.Activity, Activity>(activity);

            // todo add header of location

            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        [PUT("")]
        public HttpResponseMessage Put(int projectId,
                                       Activity model)
        {
            var activity = DbSession
                .Load<Core.Activity>(model.Id);

            Request.NotFound(activity);

            activity.Name = model.Name ?? activity.Name;
            activity.Description = model.Description ?? activity.Description;
            if (activity.Duration != model.Duration)
            {
                List<Core.Activity> domain = DbSession.Query<Core.Activity>()
                    .Where(a => a.Project == projectId.ToId("project")).ToList();
                OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }

            activity.Duration = model.Duration;
            Activity activities = Mapper.Map<Core.Activity, Activity>(activity);
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        public HttpResponseMessage Delete(int id, int projectId)
        {
            Core.Activity activity = DbSession
                .Query<Core.Activity, Activities_ByProject>()
                .FirstOrDefault(a => a.Project == projectId.ToId("project")
                                     && a.Id == id.ToId("activity"));

            Request.NotFound(activity);

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

        private void OffsetDuration(List<Core.Activity> domain, Core.Activity parent, int offset)
        {
            List<Core.Activity> children = domain.Where(a => a.Dependencies.Contains(parent.Id)).ToList();
            foreach (Core.Activity child in children)
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