﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Activity = Teamworks.Core.Activity;
using Project = Teamworks.Core.Project;
using Timelog = Teamworks.Web.Models.Api.Timelog;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities/{activityid}/timelogs")]
    public class TimelogController : RavenApiController
    {
        public IEnumerable<Timelog> Get(int projectid, int activityid)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Activity>(activityid);
            if (task == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return task.Timelogs.Select(Mapper.Map<Core.Timelog, Timelog>);
        }

        public HttpResponseMessage Post(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof(TypeConverterModelBinder))] int activityid,
            Timelog model)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Activity>(activityid);
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
        public HttpResponseMessage Put([ModelBinder(typeof(TypeConverterModelBinder))] int activityid,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       Timelog model)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);
            
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Activity>(activityid);
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
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession.Load<Activity>(activityid);
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
    }
}