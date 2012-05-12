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
using Raven.Client.Authorization;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Linq;
using Teamworks.Web.Controllers.Base;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api {
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController {
        public IEnumerable<Project> Get() {
            DbSession.SecureFor(Request.GetUserPrincipalId(), "Projects/View");
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
            DbSession.SetAuthorizationFor(project, new DocumentAuthorization
                                                   {
                                                       Permissions =
                                                           {
                                                               new DocumentPermission()
                                                               {
                                                                   Allow = true,
                                                                   Operation = "Projects/View"
                                                               }
                                                           },
                                                           Tags =  { project.Id }
                                                   });

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