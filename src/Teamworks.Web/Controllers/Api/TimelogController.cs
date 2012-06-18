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
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks/{taskid}/timelog")]
    public class TimelogController : RavenApiController
    {
        public IEnumerable<TimeEntryModel> Get(int projectid, int taskid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Task>(taskid);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return task.Timelog.Select(Mapper.Map<TimeEntry, TimeEntryModel>);
        }

        public HttpResponseMessage<TimeEntryModel> Post(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            TimeEntryModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Task>(taskid);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            //todo check for collisions?

            var timeentry = TimeEntry.Forge(model.Description, model.Date, model.Duration, Request.GetUserPrincipalId());
            timeentry.Id = task.GenerateNewTimeEntryId();
            task.Timelog.Add(timeentry);

            return new HttpResponseMessage<TimeEntryModel>(Mapper.Map<TimeEntry, TimeEntryModel>(timeentry),
                                                           HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage<TimeEntryModel> Put([ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       TimeEntryModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Task>(taskid);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var timeentry = task.Timelog.FirstOrDefault(t => t.Id.Equals(model.Id));
            if (timeentry == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            timeentry.Date = model.Date;
            timeentry.Description = model.Description;
            timeentry.Duration = model.Duration;

            return new HttpResponseMessage<TimeEntryModel>(Mapper.Map<TimeEntry, TimeEntryModel>(timeentry),
                                          HttpStatusCode.Created);
        }

        public HttpResponseMessage Delete(int id, int taskid, int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Task>(taskid);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var timeentry = task.Timelog.FirstOrDefault(t => t.Id == id);
            if (timeentry == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Timelog.Remove(timeentry);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}