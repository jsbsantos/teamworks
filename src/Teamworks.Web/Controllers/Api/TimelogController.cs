using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities/{activityid}/timelogs")]
    public class TimelogController : RavenDbApiController
    {
        /*
        [GET("api/timelogs", IsAbsoluteUrl = true)]
        public IEnumerable<Timelog_Filter.Projection> Get()
        {
            var timelogs =
                DbSession.Query<Core.Timelog, Timelog_Filter>()
                .Where(t => t.Person == Request.GetCurrentPersonId())
                .As<Timelog_Filter.Projection>().ToList();
            return timelogs;
        }

        public IEnumerable<Timelog> Get(int projectid, int activityid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var activity = DbSession.Load<Core.Activity>(activityid);
            if (activity == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Mapper.Map<IList<Core.Timelog>, IEnumerable<Timelog>>(activity.Timelogs);
        }

        public HttpResponseMessage Post(
             int projectid,
             int activityid,
            Timelog model)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Core.Activity>(activityid);
            if (task == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            DateTime date;
            if (!DateTime.TryParse(model.Date, out date))
            {
                date = DateTime.Now;
            }

            var timelog = Core.Timelog.Forge(model.Description, model.Duration, date, Request.GetCurrentPersonId());
            timelog.Id = task.GenerateNewTimeEntryId();
            task.Timelogs.Add(timelog);

            var value = Mapper.Map<Core.Timelog, Timelog>(timelog);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put( int activityid,
                                        int projectid,
                                       Timelog model)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);
            
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Core.Activity>(activityid);
            if (task == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var timelog = task.Timelogs.FirstOrDefault(t => t.Id.Equals(model.Id));
            if (timelog == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            timelog.Date = DateTime.Parse(model.Date);
            timelog.Description = model.Description;
            timelog.Duration = model.Duration;

            var value = Mapper.Map<Core.Timelog, Timelog>(timelog);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        public HttpResponseMessage Delete(int id, int activityid, int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Core.Activity>(activityid);
            if (task == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var timeentry = task.Timelogs.FirstOrDefault(t => t.Id == id);
            if (timeentry == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            task.Timelogs.Remove(timeentry);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
        */
    }
}