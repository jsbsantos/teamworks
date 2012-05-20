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
    [RoutePrefix("api/projects/{projectid}/tasks/{taskid}/timelog")]
    public class TimelogController : RavenApiController
    {
        public IEnumerable<TimelogModel> Get(int projectid, int taskid)
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

            return task.Timelog.Select(Mapper.Map<Timelog, TimelogModel>);
        }

        public HttpResponseMessage<TimelogModel> Post([ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof(TypeConverterModelBinder))] int taskid,
                                                   TimelogModel timelogModel)
        {
            var project = DbSession.Load<Project>(projectid);
            Task task = Mapper.Map<TaskModel, Task>(taskModel);
            task.Id = null;
            task.Project = project.Id;
            DbSession.Store(task);
            DbSession.SaveChanges();
            project.Tasks.Add(task.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<TaskModel>(Mapper.Map<Task, TaskModel>(task),
                                                      HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof(TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
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