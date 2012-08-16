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
        [GET("")]
        [SecureFor]
        public IEnumerable<ProjectViewModel> Get()
        {
            var projects = DbSession.Query<Project>();
            return projects.MapTo<ProjectViewModel>();
        }

        [SecureFor]
        [VetoProject(RouteValue = "id")]
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

            var uri = Url.Link("Projects_GetById",
                               new {projectId = project.Id.ToIdentifier()});

            response.Headers.Location = new Uri(uri);
            return response;
        }

        [SecureFor]
        [VetoProject(RouteValue = "id")]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            DbSession.Delete(project);

            // todo cascade remove

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [SecureFor]
        [VetoProject(RouteValue = "id")]
        [GET("{id}/accesses")]
        public IEnumerable<PersonViewModel> GetAccesses(int projectId)
        {
            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(projectId);

            var people = DbSession.Load<Person>(project.People);
            return people.MapTo<PersonViewModel>();
        }

        [SecureFor]
        [VetoProject(RouteValue = "id")]
        [POST("{id}/accesses")]
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

        [SecureFor]
        [VetoProject(RouteValue = "id")]
        [DELETE("{id}/accesses/{personId}")]
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