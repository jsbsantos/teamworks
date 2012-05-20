﻿using System;
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
            IRavenQueryable<Project> projects = DbSession.Query<Project>().Include(p => p.Tasks);
            return Mapper.Map<IQueryable<Project>, IEnumerable<ProjectModel>>(projects);
        }

        public ProjectModel Get(int id)
        {
            var project = DbSession.Include<Project>(p => p.Tasks).Load<Project>(id);
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
            DbSession.SetAuthorizationForUser(project, Request.GetCurrentPerson());

            var response = new HttpResponseMessage<ProjectModel>(Mapper.Map<Project, ProjectModel>(project),
                                                                 HttpStatusCode.Created);
            string uri = Request.RequestUri.Authority + Url.Route(null, new {id = project.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       Project project)
        {
            var p = Get<Project>(id);
            /* todo mapping */
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public HttpResponseMessage Delete(int id)
        {
            var project = Get<Project>(id);
            DbSession.Delete(project);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}