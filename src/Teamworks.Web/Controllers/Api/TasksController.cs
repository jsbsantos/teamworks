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
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Web.Models.Task;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks")]
    public class TasksController : RavenApiController
    {
        public IEnumerable<Task> Get(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<Task>(DbSession.Load<Core.Task>(project.Tasks).Select(Mapper.Map<Core.Task, Task>));
        }

        public Task Get(int id, int projectid)
        {
            var task = DbSession.Load<Core.Task>(id);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Task, Task>(task);
        }

        public HttpResponseMessage<Task> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                   Task model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = DbSession
                 .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = Core.Task.Forge(project.Id, model.Name, model.Description);
            DbSession.Store(task);
            project.Tasks.Add(task.Id);

            return new HttpResponseMessage<Task>(Mapper.Map<Core.Task, Task>(task),
                                                      HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       Task model)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Delete(int id, int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (project.Tasks.Count(t => t.Identifier() == id) == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Core.Task>(id);
            DbSession.Delete(task);
            project.Tasks.Remove(task.Id);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}