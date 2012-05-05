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
using Teamworks.Web.Models;
using Project = Teamworks.Core.Projects.Project;

namespace Teamworks.Web.Controllers.Api {
    /*
     
     todo try to inject properties
     
     example
     
     [Property<{ApiController}>({param name}, {property name})]
     
     */

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks")]
    public class TasksController : RavenApiController {
        public IEnumerable<Task> Get(int projectid) {
            var project = DbSession.Load<Project>(projectid);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<IList<Core.Projects.Task>, IEnumerable<Task>>(project.Tasks);
        }

        public Task Get(int id, int projectid) {
            var project = DbSession.Load<Project>(projectid);

            if (project == null || project.TasksReference.All(t => id != t.Identifier)) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var task = DbSession.Load<Core.Projects.Task>(id);
            if (task == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Projects.Task, Task>(task);
        }

        public HttpResponseMessage<Task> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                              Task task) {
            var project = DbSession.Load<Project>(projectid);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Core.Projects.Task t = Mapper.Map<Task, Core.Projects.Task>(task);

            t.Id = null;
            DbSession.Store(t);
            project.TasksReference.Add(t);

            task = Mapper.Map<Core.Projects.Task, Task>(t);
            string uri = Request.RequestUri.Authority + Url.Route(null, new {projectid, id = task.Id});
            var response = new HttpResponseMessage<Task>(task, HttpStatusCode.Created);
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid, Task task) {
            return null;
        }

        public HttpResponseMessage Delete(int id, int projectid) {
            return null;
        }
    }
}