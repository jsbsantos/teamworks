﻿using System.Collections.Generic;
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
    [RoutePrefix("timelogs")]
    public class TimelogsController : RavenController
    {
        [GET("")]
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
                vm.Timelogs = vm.Timelogs.Concat(activity.Timelogs.MapTo<RegisterTimelogsViewModel.Timelog>()
                                                     .Select(a =>
                                                                 {
                                                                     a.Activity = activity.MapTo<EntityViewModel>();
                                                                     a.Project = project.MapTo<EntityViewModel>();
                                                                     return a;
                                                                 })).ToList();
            }
            return View("View", vm);
        }

        [POST("projects/{projectId}/activities/{activityId}/timelogs/edit/")]
        public ActionResult Edit(int projectId, int activityId, TimelogViewModel model)
        {
            var activity = DbSession
                .Load<Activity>(activityId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var timelog = activity.Timelogs.Where(t => t.Id == model.Id).FirstOrDefault();
            timelog.Date = model.Date;
            timelog.Description = model.Description;
            timelog.Duration = model.Duration;

            return new ContentResult
            {
                Content = Utils.ToJson(timelog.MapTo<TimelogViewModel>()),
                ContentType = "application/json"
            };
        }

        [POST("projects/{projectId}/activities/{activityId}/timelogs/create/")]
        public ActionResult Create(int projectId, int activityId, TimelogViewModel model)
        {
            var activity = DbSession
                .Load<Activity>(activityId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var timelog = Timelog.Forge(model.Description, model.Duration, model.Date, DbSession.GetCurrentPerson().Id);
            timelog.Id = activity.GenerateNewTimeEntryId();
            activity.Timelogs.Add(timelog);

            return new ContentResult
            {
                Content = Utils.ToJson(timelog.MapTo<TimelogViewModel>()),
                ContentType = "application/json"
            };
        }

        [POST("projects/{projectId}/activities/{activityId}/timelogs/delete/")]
        public ActionResult Delete(int projectId, int activityId, int timelog)
        {
            var activity = DbSession
                .Load<Activity>(activityId);

            if (activity == null || activity.Project.ToIdentifier() != projectId)
                return new HttpNotFoundResult();

            var target = activity.Timelogs.Where(t => t.Id == timelog).FirstOrDefault();
            activity.Timelogs.Add(target);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}