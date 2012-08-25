using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Extensions;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController
    {
        [Secure("projects/view")]
        public IEnumerable<ProjectViewModel> Get()
        {
            var projects = DbSession.Query<Project>();
            return projects.MapTo<ProjectViewModel>();
        }

        [SecureProject("projects/view", "id")]
        public ProjectViewModel GetById(int id)
        {
            var project = DbSession.Load<Project>(id);
            if (project == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return project.MapTo<ProjectViewModel>();
        }

        public HttpResponseMessage Post(ProjectViewModel model)
        {
            var person = Request.GetCurrentPerson();
            var project = Project.Forge(model.Name, model.Description, model.StartDate);

            DbSession.Store(project);
            project.Grant(string.Empty, person);
            project.Initialize(DbSession);
            
            var value = project.MapTo<ProjectViewModel>();
            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            var uri = Url.Link("api_projects_getbyid", new { id = project.Id.ToIdentifier() });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /*
         * todo
         * 
         * According to the HTTP specification, the DELETE method must be idempotent,
         * meaning that several DELETE requests to the same URI must have the same effect
         * as a single DELETE request. Therefore, the method should not return an error
         * code if the product was already deleted.
         */
        [SecureProject("projects/delete", "id")]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            
            project.Delete(DbSession);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [GET("{projectId}/accesses")]
        [SecureProject("projects/accesses/view")]
        public IEnumerable<PersonViewModel> GetAccesses(int projectId)
        {
            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId);

            var people = DbSession.Load<Person>(project.People);
            return people.MapTo<PersonViewModel>();
        }

        [POST("{projectId}/accesses")]
        [SecureProject("projects/accesses/create")]
        public HttpResponseMessage PostAccesses(int projectId, IEnumerable<int> ids)
        {
            var people = DbSession.Query<Person>()
                .Where(p => p.Id.In(ids.Select(i => i.ToId("person"))));

            var project = DbSession.Load<Project>(projectId);
            foreach (var person in people)
                project.Grant(string.Empty, person);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [DELETE("{projectId}/accesses/{personId}")]
        [SecureProject("projects/accesses/delete")]
        public HttpResponseMessage DeleteAccess(int projectId, int personId)
        {
            var personRavenId = personId.ToId("person");
            var project = DbSession
                .Include(personRavenId)
                .Load<Project>(projectId);

            if (!project.People.Contains(personRavenId))
                Request.ThrowNotFound();

            var person = DbSession.Load<Person>(personId);
            project.Revoke(string.Empty, person);
            
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}