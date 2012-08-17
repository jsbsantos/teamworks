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
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectId}/activities/{activityId}/timelogs")]
    public class TimelogsController : RavenApiController
    {
        [NonAction]
        public Activity GetActivity(int projectId, int activityId)
        {
            var target = DbSession
                .Include<Activity>(a => a.Project)
                .Load<Activity>(activityId);

            if (target.Project.ToIdentifier() != projectId)
                Request.NotFound(target);

            return target;
        }

        public IEnumerable<TimelogViewModel> Get(int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);
            Request.NotFound(activity);
            return Mapper.Map<IList<Core.Timelog>, IEnumerable<TimelogViewModel>>(activity.Timelogs);
        }

        public HttpResponseMessage Post(
            int projectId, int activityId, TimelogViewModel model)
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

            TimelogViewModel value = Mapper.Map<Core.Timelog, TimelogViewModel>(timelog);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        public HttpResponseMessage Delete(int id, int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);

            Core.Timelog timelog = activity.Timelogs.FirstOrDefault(t => t.Id == id);
            Request.NotFound(timelog);
            activity.Timelogs.Remove(timelog);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}