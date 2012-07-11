using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Controllers.Api.Attribute;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class ProjectsController : RavenApiController
    {
        [SecureFor("/projects")]
        public IEnumerable<Project> Get()
        {
            var projects = DbSession
                .Query<Core.Project>().Customize(q => q
                    .Include<Core.Project>(p => p.Activities)
                    .Include<Core.Project>(p => p.Discussions)
                    .Include<Core.Project>(p => p.People))
                .ToList();

            return Mapper.Map<IEnumerable<Core.Project>, IEnumerable<Project>>(projects);
        }

        [SecureFor("/projects")]
        public Project Get(int id)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Discussions)
                .Include<Core.Project>(p => p.Activities)
                .Include<Core.Project>(p => p.People)
                .Load<Core.Project>(id);
            
            return Mapper.Map<Core.Project, Project>(project);
        }

        [SecureFor("/projects")]
        public HttpResponseMessage Post(Project model)
        {
            var project = Core.Project.Forge(model.Name, model.Description);

            DbSession.Store(project);
            DbSession.SetAuthorizationFor(project,
                                          new DocumentAuthorization()
                                              {
                                                  Permissions = { new DocumentPermission()
                                                                      {
                                                                          Allow = true,
                                                                          Operation = "/project",
                                                                          User = Request.GetCurrentPersonId()
                                                                      }},
                                                  Tags = new List<string>()
                                              });

            project.People.Add(Request.GetCurrentPersonId());
            var value = Mapper.Map<Core.Project, Project>(project);
            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            var uri = Uri.UriSchemeHttp + Uri.SchemeDelimiter + Request.RequestUri.Authority + Url.Route(null, new { id = project.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [SecureFor("/projects")]
        public HttpResponseMessage Delete(int id)
        {
            var project = DbSession.Load<Core.Project>(id);
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        #region People

        [SecureFor("/project")]
        [GET("{projectid}/people")]
        public IEnumerable<Person> GetPeople(int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.People)
                .Load<Core.Project>(projectid);

            var people = DbSession.Load<Core.Person>(project.People);

            return Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(people);
        }
        
        [SecureFor("/projects")]
        [POST("{projectid}/people")]
        public HttpResponseMessage Post(int projectid, Permissions model)
        {
            var project = DbSession
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                Request.NotFound();
            }

            var people = DbSession
                .Load<Core.Person>(model.Ids.Select(i => "people/" + i));
            if (people == null)
            {
                Request.NotFound();
            }

            var authorization = DbSession.GetAuthorizationFor(project);
            
            foreach (var person in people)
            {
                if (person == null)
                {
                    continue;
                }
                authorization.Permissions.Add(new DocumentPermission()
                                                      {
                                                          Allow = true,
                                                          Operation = "/project",
                                                          User = person.Id
                                                      });
            }
            
            DbSession.SetAuthorizationFor(project, authorization);
            project.People.Add(Request.GetCurrentPersonId());

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [SecureFor("/projects")]
        [DELETE("{projectid}/people/{id}")]
        public HttpResponseMessage Delete(int projectid, string id)
        {
            var personid = "people/" + id;
            var project = DbSession
                .Include(personid)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                Request.NotFound();
            }

            var authorization = DbSession.GetAuthorizationFor(project);
            var permission = authorization.Permissions
                .SingleOrDefault(p => p.User.Equals(personid));

            if (permission == null || !authorization.Permissions.Remove(permission))
            {
                Request.NotFound();
            }
            DbSession.SetAuthorizationFor(project, authorization);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion
    }
}