using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Raven.Client.Util;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenDbApiController
    {
        public ProjectsController()
        {   
        }

        public ProjectsController(IDocumentSession session)
            : base(session)
        {
        }

        [SecureFor]
        public IEnumerable<Project> Get()
        {
            RavenQueryStatistics stat;
            var projects = DbSession
                .Query<Core.Project>()
                .Statistics(out stat)
                .ToList();

            return Mapper.Map<IEnumerable<Core.Project>, IEnumerable<Project>>(projects);
        }

        [SecureFor]
        public Project Get(int id)
        {
            var project = DbSession
                .Load<Core.Project>(id);

            return Mapper.Map<Core.Project, Project>(project);
        }

        public HttpResponseMessage Post(Project model)
        {
            var project = Core.Project.Forge(model.Name, model.Description);
            DbSession.Store(project);

            project.AllowPersonAssociation();
            DbSession.SetAuthorizationFor(project, new DocumentAuthorization
                                                       {
                                                           Tags = {project.Id}
                                                       });
            var person = Request.GetCurrentPerson();
            project.People.Add(person.Id);
            person.Roles.Add(project.Id);


            var value = Mapper.Map<Core.Project, Project>(project);
            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            var uri = Uri.UriSchemeHttp + Uri.SchemeDelimiter + Request.RequestUri.Authority +
                      Url.Route(null, new {id = project.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [SecureFor]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Core.Project>(id);
            DbSession.Delete(project);

            // todo cascade remove

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #region People

        [SecureFor]
        [GET("{id}/people")]
        public IEnumerable<Person> GetPeople(int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.People)
                .Load<Core.Project>(projectid);

            var people = DbSession.Load<Core.Person>(project.People);
            return Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(people);
        }

        [SecureFor]
        [POST("{id}/people")]
        public HttpResponseMessage Post(int projectid, Permissions model)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var plural = Inflector.Pluralize("person");
            var people = DbSession
                .Load<Core.Person>(
                    model.Ids.Select(i => string.Format("{0}/{1}", plural, i)));

            foreach (var person in people.Where(p => p != null))
            {
                person.Roles.Add(project.Id);
                project.People.Add(person.Id);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [SecureFor]
        [DELETE("{id}/people/{personId}")]
        public HttpResponseMessage Delete(int id, string personId)
        {
            var project = DbSession
                .Include(string.Format("{0}/{1}",
                                       Inflector.Pluralize("person"), personId))
                .Load<Core.Project>(id);

            if (project == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var person = DbSession.Load<Core.Person>(personId);
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

        #region Task Dependencies

        [GET("{projectid}/precedences")]
        [SecureFor("/projects")]
        public DependencyGraph GetPre(int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(
                    Request.CreateResponse(HttpStatusCode.NotFound));
            }

            //var relations = project.DependencyGraph();
            List<int[]> relations = null;
            var elements = DbSession.Load<Core.Activity>(project.Activities)
                .Select(Mapper.Map<Core.Activity, DryActivity>)
                .ToList();

            return new DependencyGraph() {Elements = elements, Relations = relations};
        }

        #endregion
    }
}