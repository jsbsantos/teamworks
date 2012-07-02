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
using Raven.Client.Linq;
using Teamworks.Core.Projects;
using Teamworks.Web.Controllers.Api.Handlers;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/events")]
    public class EventsController : RavenApiController
    {
        [SecureFor("/projects/view")]
        public IEnumerable<ProjectModel> Get()
        {
            var projects = DbSession.Query<Project>().Include(p => p.Tasks).ToList();
            return Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectModel>>(projects);
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

        public HttpResponseMessage<ProjectModel> Post(ProjectModel model)
        {
            Project project = Project.Forge(model.Name, model.Description);
            DbSession.Store(project);
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
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
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