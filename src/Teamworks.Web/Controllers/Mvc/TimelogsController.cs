using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("projects/{projectId}/activities/{activityId}/timelogs")]
    public class TimelogsController : AppController
    {
        [GET("timelogs", IsAbsoluteUrl = true)]
        public ActionResult Get()
        {
            List<Activity> activities = DbSession.Query<Activity>()
                .Customize(c => c.Include<Activity>(a => a.Project))
                .ToList();

            var vm = new RegisterTimelogsViewModel();
            foreach (var act in activities)
            {
                var activity = act;
                var project = DbSession.Load<Project>(activity.Project);
                var option = new RegisterTimelogsViewModel.Typeahead
                    {
                        Activity = activity.MapTo<EntityViewModel>(),
                        Project = project.MapTo<EntityViewModel>()
                    };

                vm.Options.Add(option);
                vm.Timelogs = vm.Timelogs
                    .Concat(activity.Timelogs.MapTo<RegisterTimelogsViewModel.Timelog>()
                                .Select(a =>
                                    {
                                        a.Activity = activity.MapTo<EntityViewModel>();
                                        a.Project = project.MapTo<EntityViewModel>();
                                        return a;
                                    }))
                    .ToList();
            }

            vm.Timelogs = vm.Timelogs
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.Project.Id)
                .ThenBy(x => x.Activity.Id).ToList();
            return View("View", vm);
        }

        [POST("edit")]
        public ActionResult Edit(int projectId, int activityId, TimelogViewModel model)
        {
            var activity = DbSession
                .Include(projectId.ToId("project"))
                .Load<Activity>(activityId);

            var project = DbSession
                .Load<Project>(projectId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var timelog = activity.Timelogs.Where(t => t.Id == model.Id).FirstOrDefault();
            timelog.Date = model.Date;
            timelog.Description = model.Description;
            timelog.Duration = model.Duration;

            var result = timelog.MapTo<RegisterTimelogsViewModel.Timelog>();
            result.Activity = activity.MapTo<EntityViewModel>();
            result.Project = project.MapTo<EntityViewModel>();

            return new JsonNetResult {Data = result};
        }

        [POST("create")]
        public ActionResult Create(int projectId, int activityId, TimelogViewModel model)
        {
            var activity = DbSession
                .Include(projectId.ToId("project"))
                .Load<Activity>(activityId);

            var project = DbSession
                .Load<Project>(projectId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var timelog = Timelog.Forge(model.Description, model.Duration, model.Date, DbSession.GetCurrentPerson().Id);
            timelog.Id = activity.GenerateNewTimeEntryId();
            activity.Timelogs.Add(timelog);

            var result = timelog.MapTo<RegisterTimelogsViewModel.Timelog>();
            result.Activity = activity.MapTo<EntityViewModel>();
            result.Project = project.MapTo<EntityViewModel>();

            return new JsonNetResult {Data = result};
        }

        [POST("{timelogId}/delete")]
        public ActionResult Delete(int projectId, int activityId, int timelogId)
        {
            var activity = DbSession
                .Load<Activity>(activityId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var target = activity.Timelogs.Where(t => t.Id == timelogId).FirstOrDefault();
            activity.Timelogs.Remove(target);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}