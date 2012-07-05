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
            var projects = DbSession.Query<Core.Project>()
                .Customize(q => q.Include<Core.Project>(p => p.Activities))
                .ToList();

            return Mapper.Map<IEnumerable<Core.Project>, IEnumerable<Project>>(projects);
        }

        [SecureFor("/projects")]
        public Project Get(int id)
        {
            var project = DbSession.Include<Core.Project>(p => p.Activities)
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
                                                  Tags = {Request.GetCurrentPersonId()}
                                              });


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

        [SecureFor("/projects")]
        [POST("{projectid}/people/{identifier}")]
        public HttpResponseMessage Post(int projectid, string identifier)
        {
            var personid = "people/" + identifier;
            var project = DbSession
                .Include(personid)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                Request.NotFound();
            }

            var person = DbSession.Load<Core.Person>(personid);
            if (person == null)
            {
                Request.NotFound();
            }

            var authorization = DbSession.GetAuthorizationFor(project);
            authorization.Tags.Add(person.Id);
            DbSession.SetAuthorizationFor(project, authorization);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [SecureFor("/projects")]
        [DELETE("{projectid}/people/{identifier}")]
        public HttpResponseMessage Delete(int projectid, string identifier)
        {
            var personid = "people/" + identifier;
            var project = DbSession
                .Include(personid)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                Request.NotFound();
            }
            var person = DbSession.Load<Core.Person>(personid);
            var authorization = DbSession.GetAuthorizationFor(project);
            if (!authorization.Tags.Remove(person.Id))
            {
                Request.NotFound();
            }
            DbSession.SetAuthorizationFor(project, authorization);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion
    }
}