using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions.Api;
using Timelog = Teamworks.Web.Models.Api.Timelog;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectId}/activities/{activityId}/timelogs")]
    public class TimelogsController : RavenApiController
    {
        public TimelogsController()
        {
        }

        public TimelogsController(IDocumentSession session)
            : base(session)
        {
        }

        [NonAction]
        public Activity GetActivity(int projectId, int activityId)
        {
            var target = DbSession
                .Include<Activity>(a => a.Project)
                .Load<Activity>(activityId);

            if (target.Project.ToIdentifier() != projectId)
                Request.ThrowNotFoundIfNull(target);

            return target;
        }

        public IEnumerable<Timelog> Get(int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);
            Request.ThrowNotFoundIfNull(activity);
            return Mapper.Map<IList<Core.Timelog>, IEnumerable<Timelog>>(activity.Timelogs);
        }

        public HttpResponseMessage Post(
            int projectId, int activityId, Timelog model)
        {
            Activity activity = GetActivity(projectId, activityId);

            DateTime date;
            if (!DateTime.TryParse(model.Date, out date))
            {
                date = DateTime.Now;
            }

            Core.Timelog timelog = Core.Timelog.Forge(model.Description, model.Duration, date,
                                                      Request.GetCurrentPersonId());
            timelog.Id = activity.GenerateNewTimeEntryId();
            activity.Timelogs.Add(timelog);

            Timelog value = Mapper.Map<Core.Timelog, Timelog>(timelog);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        public HttpResponseMessage Delete(int id, int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);

            Core.Timelog timelog = activity.Timelogs.FirstOrDefault(t => t.Id == id);
            Request.ThrowNotFoundIfNull(timelog);
            activity.Timelogs.Remove(timelog);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}