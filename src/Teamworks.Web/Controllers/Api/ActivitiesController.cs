using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
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
                Request.NotFound();

            var activities = DbSession.Load<Core.Activity>(project.Activities);
            return activities.Select(Mapper.Map<Core.Activity, Activity>);
        }

        public Activity Get(int id, int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
                Request.NotFound();

            var activity = DbSession.Load<Core.Activity>(id);
            if (activity == null)
                Request.NotFound();

            return Mapper.Map<Core.Activity, Activity>(activity);
        }

        public HttpResponseMessage Post(int projectid,
                                        Activity model)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);

            if (project == null)
                Request.NotFound();

            var activity = Core.Activity.Forge(project.Id, model.Name, model.Description, model.Duration);
            DbSession.Store(activity);

            if (model.Dependencies != null)
                activity.Dependencies = model.Dependencies;
            project.Activities.Add(activity.Id);

            var domain = DbSession.Load<Core.Activity>(project.Activities).ToList();
            activity.StartDate = project.StartDate.AddMinutes(GetAccumulatedDuration(domain, activity));

            var activities = Mapper.Map<Core.Activity, Activity>(activity);
            return Request.CreateResponse(HttpStatusCode.Created, activities);
        }

        [PUT("")]
        public HttpResponseMessage Put(int projectid,
                                       Activity model)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);

            if (project == null)
                Request.NotFound();

            var activity = DbSession.Load<Core.Activity>(int.Parse(model.Id));
            if (activity == null)
                Request.NotFound();

            activity.Name = model.Name;
            activity.Description = model.Description;
            if (activity.Duration != model.Duration)
            {
                var domain = DbSession.Load<Core.Activity>(project.Activities).ToList();
                OffsetDuration(domain, activity, model.Duration - activity.Duration);
            }
            activity.Duration = model.Duration;

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

        private double GetAccumulatedDuration(List<Core.Activity> domain, Core.Activity activity, int duration = 0)
        {
            var parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;
             
            return  parent.Dependencies.Count == 0
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
        public List<ActivityRelation> GetPre(int id, int projectid)
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
                Request.CreateResponse(HttpStatusCode.NotModified); //todo is valid response code?

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
                Request.CreateResponse(HttpStatusCode.NotModified); //todo is valid response code?

            activity.Dependencies.Remove(depid);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion

        #region People
        
        #endregion
    }
}