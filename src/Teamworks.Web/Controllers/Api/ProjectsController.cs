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
using Teamworks.Web.Helpers;

namespace Teamworks.Web.Controllers.Api {
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController {
        public IEnumerable<Models.Project> Get() {
            return Mapper.Map<IQueryable<Project>, IEnumerable<Models.Project>>(DbSession.Query<Project>());
        }

        public Models.Project Get(int id) {
            var project = DbSession.Load<Project>(id);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Project, Models.Project>(project);
        }

        public HttpResponseMessage<Models.Project> Post(Models.Project project) {
            var proj = Mapper.Map<Models.Project, Project>(project);

            proj.Id = null;
            DbSession.Store(proj);

            project = Mapper.Map<Project, Models.Project>(proj);
            var uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            var response = new HttpResponseMessage<Models.Project>(project, HttpStatusCode.Created);
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id, Models.Project project) {
            var proj = DbSession.Load<Project>(id);
            if (proj == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            /* todo mapping */
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Delete(int id) {
            var project = DbSession.Load<Project>(id);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}