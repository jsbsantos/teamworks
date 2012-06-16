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
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks")]
    public class TasksController : RavenApiController
    {
        public IEnumerable<TaskModel> Get(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<TaskModel>(DbSession.Load<Task>(project.Tasks).Select(Mapper.Map<Task, TaskModel>));
        }

        public TaskModel Get(int id, int projectid)
        {
            var task = DbSession.Load<Task>(id);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Task, TaskModel>(task);
        }

        public HttpResponseMessage<TaskModel> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                   TaskModel taskModel)
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

            Task task = Task.Forge(project.Id, taskModel.Name, taskModel.Description);
            DbSession.Store(task);
            project.Tasks.Add(task.Id);

            return new HttpResponseMessage<TaskModel>(Mapper.Map<Task, TaskModel>(task),
                                                      HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       TaskModel taskModel)
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

            var task = DbSession.Load<Task>(id);
            DbSession.Delete(task);
            project.Tasks.Remove(task.Id);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}