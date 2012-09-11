using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [SecureProject("projects/view")]
    [RoutePrefix("api/projects/{projectId}/activities/{activityId}/timelogs")]
    public class TimelogsController : AppApiController
    {
        [NonAction]
        private Activity GetActivity(int projectId, int activityId)
        {
            var target = DbSession
                .Include<Activity>(a => a.Project)
                .Load<Activity>(activityId);

            if (target == null || target.Project.ToIdentifier() != projectId)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return target;
        }

        public IEnumerable<TimelogViewModel> Get(int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);

            return Mapper.Map<IList<Timelog>, IEnumerable<TimelogViewModel>>(activity.Timelogs);
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

            Timelog timelog = Timelog.Forge(model.Description, model.Duration, date,
                                                      Request.GetCurrentPersonId());
            timelog.Id = activity.GenerateNewTimeEntryId();
            activity.Timelogs.Add(timelog);

            TimelogViewModel value = Mapper.Map<Timelog, TimelogViewModel>(timelog);
            value.Activity = activityId;
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        public HttpResponseMessage Put(
            int projectId, int activityId, TimelogViewModel model)
        {
            Activity activity = GetActivity(projectId, activityId);

            DateTime date;
            if (!DateTime.TryParse(model.Date, out date))
            {
                date = DateTime.Now;
            }

            var timelog = activity.Timelogs.FirstOrDefault(t => t.Id == model.Id);
            if(timelog == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            timelog.Date = date;
            timelog.Description = model.Description;
            timelog.Duration = model.Duration;

            TimelogViewModel value = Mapper.Map<Timelog, TimelogViewModel>(timelog);
            value.Activity = activityId;
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        public HttpResponseMessage Delete(int id, int projectId, int activityId)
        {
            Activity activity = GetActivity(projectId, activityId);

            var timelog = activity.Timelogs.FirstOrDefault(t => t.Id == id);
            if (timelog != null)
                activity.Timelogs.Remove(timelog);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}