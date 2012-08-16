using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor]
    [RoutePrefix("api/projects/{projectId}/activities")]
    public class ActivitiesController : RavenApiController
    {
        [GET("")]
        public IEnumerable<ActivityViewModel> Get(int projectId)
        {
            var activities = DbSession
                .Query<Activity, Activities_ByProject>()
                .Where(a => a.Project == projectId.ToId("project"))
                .ToList();

            return activities.MapTo<ActivityViewModel>();
        }

        [GET("{activityId}")]
        public ActivityViewModel GetById(int projectId, int activityId)
        {
            var project = projectId.ToId("project");
            var activity = DbSession.Include(projectId.ToId(""))
                .Load<Activity>(activityId);

            if (activity == null || activity.Project != project)
                Request.ThrowNotFound();
            
            return activity.MapTo<ActivityViewModel>();
        }

        [POST("")]
        public HttpResponseMessage Post(int projectId, ActivityViewModel model)
        {
            var project = DbSession
                .Load<Project>(projectId);

            var activity = Core.Activity.Forge(projectId, model.Name, model.Description, model.Duration);

            DbSession.Store(activity);
            DbSession.SetAuthorizationFor(activity, new DocumentAuthorization
                                                        {
                                                            Tags = {project.Id}
                                                        });

            // calculate dependency graph 
            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;

            List<Activity> domain = DbSession.Query<Activity>()
                .Where(a => a.Project == project.Id).ToList();

            //activity.StartDate = project.StartDate.AddMinutes(GetAccumulatedDuration(domain, activity));

            var activitiesViewModel = activity.MapTo<ActivityViewModel>();

            // todo add header of location

            return Request.CreateResponse(HttpStatusCode.Created, activitiesViewModel);
        }

        [PUT("{activityId}")]
        public HttpResponseMessage Put(int projectId, int activityId,
                                       ActivityViewModel model)
        {
            var activity = DbSession
                .Load<Activity>(model.Id);

            Request.ThrowNotFoundIfNull(activity);

            activity.Name = model.Name ?? activity.Name;
            activity.Description = model.Description ?? activity.Description;
            if (activity.Duration != model.Duration)
            {
                List<Activity> domain = DbSession.Query<Core.Activity>()
                    .Where(a => a.Project == projectId.ToId("project")).ToList();
               // OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }

            activity.Duration = model.Duration;
            ActivityViewModel activitiesViewModel = Mapper.Map<Core.Activity, ActivityViewModel>(activity);
            return Request.CreateResponse(HttpStatusCode.Created, activitiesViewModel);
        }

        [DELETE("{activityId}")]
        public HttpResponseMessage Delete(int projectId, int activityId)
        {
            var project = projectId.ToId("project");
            var activity = DbSession.Include(projectId.ToId(""))
                .Load<Activity>(activityId);

            if (activity == null || activity.Project != project)
                Request.ThrowNotFound();
            
            DbSession.Delete(activity);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}