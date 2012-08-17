using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController
    {
        [SecureFor]
        public IEnumerable<ProjectViewModel> Get()
        {
            var projects = DbSession.Query<Project>();
            return projects.MapTo<ProjectViewModel>();
        }

        [SecureFor(Priority = 1)]
        [VetoProject(RouteValue = "id", Priority = 2)]
        public ProjectViewModel GetById(int id)
        {
            var project = DbSession.Load<Project>(id);
            return project.MapTo<ProjectViewModel>();
        }

        public HttpResponseMessage Post(ProjectViewModel model)
        {
            var person = Request.GetCurrentPerson();
            var project = Project.Forge(model.Name, model.Description, model.StartDate);

            DbSession.Store(project);
            DbSession.GrantAccessToProject(project, person);

            var value = project.MapTo<ProjectViewModel>();
            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            var uri = Url.Link("Projects_GetById", new { projectId = project.Id.ToIdentifier() });
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
        [SecureFor(Priority = 1)]
        [VetoProject(RouteValue = "id", Priority = 2)]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            DbSession.Delete(project);

            // todo cascade remove

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [GET("{id}/accesses")]
        [SecureFor(Priority = 1)]
        [VetoProject(RouteValue = "id", Priority = 2)]
        public IEnumerable<PersonViewModel> GetAccesses(int projectId)
        {
            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId);

            var people = DbSession.Load<Person>(project.People);
            return people.MapTo<PersonViewModel>();
        }

        [POST("{id}/accesses")]
        [SecureFor(Priority = 1)]
        [VetoProject(RouteValue = "id", Priority = 2)]
        public HttpResponseMessage PostAccesses(int id, IEnumerable<int> ids)
        {
            var people = DbSession
                .Query<Person>()
                .Where(p => p.Id.In(ids.Select(i => i.ToId("person"))));

            var project = DbSession.Load<Project>(id);
            foreach (var person in people)
                DbSession.GrantAccessToProject(project, person);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [DELETE("{id}/accesses/{personId}")]
        [SecureFor(Priority = 1)]
        [VetoProject(RouteValue = "id", Priority = 2)]
        public HttpResponseMessage DeleteAccess(int id, int personId)
        {
            var personRavenId = personId.ToId("person");
            var project = DbSession
                .Include(personRavenId)
                .Load<Project>(id);

            if (!project.People.Contains(personRavenId))
                Request.ThrowNotFound();

            var person = DbSession.Load<Person>(personId);
            DbSession.RevokeAccessToProject(project, person);
            
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}