using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities")]
    public class ActivitiesController : RavenDbApiController
    {
        #region General
        public IEnumerable<Activity> Get(int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var activities = DbSession.Load<Core.Activity>(project.Activities);
            return activities.Select(Mapper.Map<Core.Activity, Activity>);
        }

        public Activity Get(int id, int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var activity = DbSession.Load<Core.Activity>(id);
            if (activity == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Mapper.Map<Core.Activity, Activity>(activity);
        }

        public HttpResponseMessage Post(int projectid,
                                        Activity model)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var activity = Core.Activity.Forge(project.Id, model.Name, model.Description);
            DbSession.Store(activity);
            project.Activities.Add(activity.Id);
            var activities = Mapper.Map<Core.Activity, Activity>(activity);
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        public HttpResponseMessage Delete(int id, int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (project.Activities.Any(t => t.Identifier() == id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Core.Activity>(id);
            DbSession.Delete(task);
            project.Activities.Remove(task.Id);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Precedence

        [GET("{id}/precedences")]
        public List<int[]> GetPre(int id, int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            var activity = DbSession.Load<Core.Activity>(id);

            if (project == null || activity == null || !project.Activities.Any(t => t.Identifier() == id))
                Request.NotFound();

            //var graph = activity.DependencyGraph();
            //return graph;
            return null;
        }

        [POST("{id}/precedences/{precedenceid}")]
        public HttpResponseMessage PostPre(int id, int projectid,
                                           int precedenceid)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);
            var activity = DbSession.Load<Core.Activity>(id);

            if (project == null || activity == null || !project.Activities.Any(t => t.Identifier() == id))
                Request.NotFound();

            var depid = "activities/" + precedenceid;
            if (activity.Dependencies.Contains(depid))
                Request.CreateResponse(HttpStatusCode.NotModified);//todo is valid response code?

            activity.Dependencies.Add(depid);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [DELETE("{id}/precedences/{precedenceid}")]
        public HttpResponseMessage Delete(int id, int projectid, int precedenceid)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);
            var activity = DbSession.Load<Core.Activity>(id);

            if (project == null || activity == null || !project.Activities.Any(t => t.Identifier() == id))
                Request.NotFound();

            var depid = "activities/" + precedenceid;
            if (!activity.Dependencies.Contains(depid))
                Request.CreateResponse(HttpStatusCode.NotModified);//todo is valid response code?

            activity.Dependencies.Remove(depid);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion

        #region People
        
        #endregion
    }
}