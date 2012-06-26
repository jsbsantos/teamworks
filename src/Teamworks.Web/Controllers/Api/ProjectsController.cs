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
using Teamworks.Web.Controllers.Api.Attribute;
using Project = Teamworks.Web.Models.Project;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController
    {
        [SecureFor("/projects/view")]
        public IEnumerable<Project> Get()
        {
            var projects = DbSession.Query<Core.Project>().Include(p => p.Activities).ToList();
            return Mapper.Map<IEnumerable<Core.Project>, IEnumerable<Project>>(projects);
        }

        [SecureFor("/projects/view")]
        public Project Get(int id)
        {
            var project = DbSession.Include<Core.Project>(p => p.Activities).Load<Core.Project>(id);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Project, Project>(project);
        }

        public HttpResponseMessage<Project> Post(Project model)
        {
            var project = Core.Project.Forge(model.Name, model.Description);
            DbSession.Store(project);
            var response = new HttpResponseMessage<Project>(Mapper.Map<Core.Project, Project>(project),
                                                                 HttpStatusCode.Created);
            
            var uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage Delete(int id)
        {
            var project = Get<Core.Project>(id);
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}