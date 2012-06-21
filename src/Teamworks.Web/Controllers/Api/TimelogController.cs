using System;
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
using Teamworks.Core;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Core.Task;
using Timelog = Teamworks.Web.Models.Timelog;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks/{taskid}/timelog")]
    public class TimelogController : RavenApiController
    {
        public IEnumerable<Timelog> Get(int projectid, int taskid)
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

            return task.Timelogs.Select(Mapper.Map<Core.Timelog, Timelog>);
        }

        public HttpResponseMessage<Timelog> Post(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            Timelog model)
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

            var timeentry = Core.Timelog.Forge(model.Description, model.Date, model.Duration, Request.GetUserPrincipalId());
            timeentry.Id = task.GenerateNewTimeEntryId();
            task.Timelogs.Add(timeentry);

            return new HttpResponseMessage<Timelog>(Mapper.Map<Core.Timelog, Timelog>(timeentry),
                                                           HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage<Timelog> Put([ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       Timelog model)
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

            var timeentry = task.Timelogs.FirstOrDefault(t => t.Id.Equals(model.Id));
            if (timeentry == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            timeentry.Date = model.Date;
            timeentry.Description = model.Description;
            timeentry.Duration = model.Duration;

            return new HttpResponseMessage<Timelog>(Mapper.Map<Core.Timelog, Timelog>(timeentry),
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

            var timeentry = task.Timelogs.FirstOrDefault(t => t.Id == id);
            if (timeentry == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.Timelogs.Remove(timeentry);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}