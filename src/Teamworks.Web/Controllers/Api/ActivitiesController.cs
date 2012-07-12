using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Teamworks;
using Activity = Teamworks.Web.Models.Api.Activity;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities")]
    public class ActivitiesController : RavenApiController
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
            return new List<Activity>(activities.Select(Mapper.Map<Core.Activity, Activity>));
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

            if (project.Activities.Count(t => t.Identifier() == id) == 0)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Core.Activity>(id);
            DbSession.Delete(task);
            project.Activities.Remove(task.Id);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        #endregion
    }
}