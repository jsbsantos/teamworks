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
using Teamworks.Web.Controllers.Base;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api {
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks")]
    public class TasksController : RavenApiController {
        public IEnumerable<Models.Task> Get(int projectid) {
            var project = DbSession.Load<Core.Projects.Project>(projectid);
            foreach (var i in Enumerable.Range(1, 5)) {
                var task = Core.Projects.Task.Forge(string.Format("task {0}", i), string.Format("description of target {0}", i));
                DbSession.Store(task);
                task.ProjectId = project.Id;
                project.TaskIds.Add(task.Id);
            }
            return null;
        }

        public Task Get(int id, int projectid) {
            var task = DbSession.Load<Core.Projects.Task>(id);
            if (task == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Projects.Task, Task>(task);
        }

        public HttpResponseMessage<Models.Task> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                              Models.Task task) {



            return null;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid, Models.Task task) {
            return null;
        }

        public HttpResponseMessage Delete(int id, int projectid) {
            return null;
        }
    }
}