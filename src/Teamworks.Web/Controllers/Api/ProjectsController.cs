using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core;
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
        [SecureFor]
        public IEnumerable<ProjectViewModel> Get()
        {
            var projects = DbSession.Query<Project>()
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return projects.MapTo<ProjectViewModel>();
        }

        [SecureFor]
        public ProjectViewModel GetById(int id)
        {
            var project = DbSession.Load<Project>(id);
            return project.MapTo<ProjectViewModel>();
        }

        public HttpResponseMessage Post(ProjectViewModel model)
        {
            var project = Project.Forge(model.Name,
                                        model.Description,
                                        model.StartDate);
            DbSession.Store(project);

            project.AllowPersonAssociation();
            DbSession.SetAuthorizationFor(project, new DocumentAuthorization
                                                       {
                                                           Tags = {project.Id}
                                                       });
            var person = Request.GetCurrentPerson();

            project.People.Add(person.Id);
            person.Roles.Add(project.Id);

            var response = Request.CreateResponse(HttpStatusCode.Created,
                                                  project.MapTo<ProjectViewModel>());

            var uri = Url.Link("Projects_GetById", new {id = project.Id.ToIdentifier()});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [SecureFor]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Project>(id);
            DbSession.Delete(project);

            // todo cascade remove

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #region People

        [SecureFor]
        [GET("{id}/people")]
        public IEnumerable<PersonViewModel> GetPeopleByProjectId(int id)
        {
            var project = DbSession
                .Include<Project>(p => p.People)
                .Load<Project>(id);

            var people = DbSession.Load<Person>(project.People);
            return people.MapTo<PersonViewModel>();
        }

        [SecureFor]
        [POST("{id}/people")]
        public HttpResponseMessage PostPermission(int id, Permissions model)
        {
            var project = DbSession
                .Load<Project>(id);

            var people = DbSession
                .Load<Person>(model.Ids.Select(i => i.ToId("person")))
                .Where(p => p != null);

            foreach (var person in people)
            {
                person.Roles.Add(project.Id);
                project.People.Add(person.Id);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [SecureFor]
        [DELETE("{id}/people/{personId}")]
        public HttpResponseMessage DeletePermission(int id, string personId)
        {
            var project = DbSession
                .Include(id.ToId("project"))
                .Load<Project>(id);

            if (project == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var person = DbSession.Load<Person>(personId);
            if (!project.People.Remove(person.Id))
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }
            person.Roles.Remove(project.Id);

            // todo cascade remove

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion
    }
}