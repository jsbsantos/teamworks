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
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController
    {
        public IEnumerable<ProjectModel> Get()
        {
            DbSession.SecureFor(Request.GetUserPrincipalId(), "Projects/View");
            IRavenQueryable<Project> projects = DbSession.Query<Project>().Include(p => p.TaskIds);
            return Mapper.Map<IQueryable<Project>, IEnumerable<ProjectModel>>(projects);
        }

        public ProjectModel Get(int id)
        {
            var project = DbSession.Include<Project>(p => p.TaskIds).Load<Project>(id);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Project, ProjectModel>(project);
        }

        public HttpResponseMessage<ProjectModel> Post(ProjectModel form)
        {
            Project project = Project.Forge(form.Name, form.Description);
            DbSession.Store(project);
            DbSession.SetAuthorizationFor(project, new DocumentAuthorization
                                                       {
                                                           Permissions =
                                                               {
                                                                   new DocumentPermission
                                                                       {
                                                                           Allow = true,
                                                                           Operation = "Projects/View"
                                                                       }
                                                               },
                                                           Tags = {project.Id}
                                                       });

            var response = new HttpResponseMessage<ProjectModel>(Mapper.Map<Project, ProjectModel>(project),
                                                                 HttpStatusCode.Created);
            string uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx"/>
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       Project project)
        {
            var proj = DbSession.Load<Project>(id);
            if (proj == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            /* todo mapping */
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}