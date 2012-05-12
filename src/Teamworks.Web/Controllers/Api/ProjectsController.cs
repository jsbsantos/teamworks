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
using Raven.Client.Linq;
using Teamworks.Web.Controllers.Base;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api {
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController {
        public IEnumerable<Project> Get() {
            var projects = DbSession.Query<Core.Projects.Project>().Include(p => p.TaskIds);
            return Mapper.Map<IQueryable<Core.Projects.Project>, IEnumerable<Project>>(projects);
        }

        public Project Get(int id) {
            var project = DbSession.Include<Core.Projects.Project>(p => p.TaskIds).Load<Core.Projects.Project>(id);
            if (project == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Projects.Project, Project>(project);
        }

        public HttpResponseMessage<Project> Post(Project form) {
            var project = Core.Projects.Project.Forge(form.Name, form.Description);
            DbSession.Store(project);

            var response = new HttpResponseMessage<Project>(Mapper.Map<Core.Projects.Project, Project>(project),
                                                            HttpStatusCode.Created);
            var uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       Core.Projects.Project project) {
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