using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Business;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Extensions.Api;
using Activity = Teamworks.Web.Models.Api.Activity;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor]
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectId}/activities")]
    public class ActivitiesController : RavenApiController
    {
        #region General

        private ActivityServices ActivityServices { get; set; }

        public ActivitiesController()
        {
            ActivityServices = new Lazy<ActivityServices>(() => new ActivityServices() {DbSession = DbSession}).Value;
        }

        public ActivitiesController(IDocumentSession session)
            : base(session)
        {
            ActivityServices = new Lazy<ActivityServices>(() => new ActivityServices() {DbSession = session}).Value;
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

            Request.ThrowNotFoundIfNull(activity);
            return Mapper.Map<Core.Activity, Activity>(activity);
        }

        public HttpResponseMessage Post(int projectId, Activity model)
        {
            var project = DbSession
                .Load<Project>(projectId);

            Core.Activity activity = Core.Activity.Forge(project.Id, model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
                {
                    Tags = {project.Id}
                });

            // calculate dependency graph 
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;

            List<Core.Activity> domain = DbSession.Query<Core.Activity>()
                .Where(a => a.Project == project.Id).ToList();

            activity.StartDate = project.StartDate.AddMinutes(GetAccumulatedDuration(domain, activity));

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

            Request.ThrowNotFoundIfNull(activity);

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

            Request.ThrowNotFoundIfNull(activity);

            DbSession.Delete(activity);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Precedence

        [POST("{id}/precedences")]
        public HttpResponseMessage PostPre(int id, int projectId,
                                           int[] precedences)
        {
            if (!ActivityServices.AddPrecedence(id, projectId, precedences))
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

        #region Private

        private double GetAccumulatedDuration(List<Core.Activity> domain, Core.Activity activity, int duration = 0)
        {
            Core.Activity parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;

            return /*parent.Dependencies.Count == 0
                       ? duration
                       : */
                GetAccumulatedDuration(domain, parent, duration + parent.Duration);
        }

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