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
using Teamworks.Core;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Activity = Teamworks.Web.Models.Api.Activity;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/activities")]
    public class ActivitiesController : RavenApiController
    {
        public IEnumerable<Activity> Get(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<Activity>(DbSession.Load<Core.Activity>(project.Activities).Select(Mapper.Map<Core.Activity, Activity>));
        }

        public Activity Get(int id, int projectid)
        {
            var task = DbSession.Load<Core.Activity>(id);
            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Core.Activity, Activity>(task);
        }

        public HttpResponseMessage<Activity> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                   Activity model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = DbSession
                 .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = Core.Activity.Forge(project.Id, model.Name, model.Description);
            DbSession.Store(task);
            project.Activities.Add(task.Id);

            return new HttpResponseMessage<Activity>(Mapper.Map<Core.Activity, Activity>(task),
                                                      HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof (TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                       Activity model)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Delete(int id, int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (project.Activities.Count(t => t.Identifier() == id) == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession.Load<Core.Activity>(id);
            DbSession.Delete(task);
            project.Activities.Remove(task.Id);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}