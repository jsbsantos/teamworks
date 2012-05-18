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
                .Include<Project>(p => p.TaskIds)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<TaskModel>(DbSession.Load<Task>(project.TaskIds).Select(Mapper.Map<Task, TaskModel>));
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
            var project = DbSession.Load<Project>(projectid);
            Task task = Mapper.Map<TaskModel, Task>(taskModel);
            task.Id = null;
            task.ProjectId = project.Id;
            DbSession.Store(task);
            DbSession.SaveChanges();
            project.TaskIds.Add(task.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<TaskModel>(Mapper.Map<Task, TaskModel>(task),
                                                      HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       TaskModel taskModel)
        {
            return null;
        }

        public HttpResponseMessage Delete(int id, int projectid)
        {
            return null;
        }
    }
}