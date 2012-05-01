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

namespace Teamworks.Web.Controllers.Api {
    
    /*
     
     todo try to inject properties
     
     example
     
     [Property<{ApiController}>({param name}, {property name})]
     
     */

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/tasks")]
    public class TasksController : RavenApiController {
        public IEnumerable<Models.Task> Get(int projectid) {
            var project = DbSession.Load<Project>(projectid);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<IList<Task>, IEnumerable<Models.Task>>(project.Tasks);
        }

        [GET("{id}")]
        public Models.Task Get(int projectid, int id) {
            var project = DbSession.Load<Project>(projectid);
            
            if (project == null || project.TasksReference.All(t => id != t.Identifier)) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var task = DbSession.Load<Task>(id);
            if (task == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Task, Models.Task>(task);
        }

        public HttpResponseMessage<Models.Task> Post([ModelBinder(typeof(TypeConverterModelBinder))]int projectid, Models.Task task)
        {
            var project = DbSession.Load<Project>(projectid);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
            var t = Mapper.Map<Models.Task, Task>(task);

            t.Id = null;
            DbSession.Store(t);
            project.TasksReference.Add(t);
            
            task = Mapper.Map<Task, Models.Task>(t);
            var uri = Request.RequestUri.Authority + Url.Route(null, new {projectid = projectid, id = task.Id});
            var response = new HttpResponseMessage<Models.Task>(task, HttpStatusCode.Created);
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        [PUT("{id}")]
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int id, Models.Task task) {
            return null;
        }

        [DELETE("{id}")]
        public HttpResponseMessage Delete(int projectid, int id) {
            return null;
        }
    }
}