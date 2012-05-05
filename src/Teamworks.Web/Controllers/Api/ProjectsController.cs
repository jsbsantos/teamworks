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

namespace Teamworks.Web.Controllers.Api {
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController {
        public IEnumerable<Project> Get() {
            return
                Mapper.Map<IQueryable<Core.Projects.Project>, IEnumerable<Project>>(
                    DbSession.Query<Core.Projects.Project>());
        }

        public Project Get(int id) {
            var project = DbSession.Load<Core.Projects.Project>(id);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Projects.Project, Project>(project);
        }

        public HttpResponseMessage<Project> Post(Project project) {
            Core.Projects.Project proj = Mapper.Map<Project, Core.Projects.Project>(project);

            proj.Id = null;
            DbSession.Store(proj);

            project = Mapper.Map<Core.Projects.Project, Project>(proj);
            string uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            var response = new HttpResponseMessage<Project>(project, HttpStatusCode.Created);
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id, Project project) {
            var proj = DbSession.Load<Core.Projects.Project>(id);
            if (proj == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            /* todo mapping */
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Delete(int id) {
            var project = DbSession.Load<Core.Projects.Project>(id);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}