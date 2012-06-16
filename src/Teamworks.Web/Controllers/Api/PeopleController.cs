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
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class PeopleController : RavenApiController
    {
        #region General
        //todo unbound (limited by raven) result set - page results?
        [GET("people")]
        public IEnumerable<PersonModel> GetPeople()
        {
            return new List<PersonModel>(
                    DbSession.Query<Person>().Select(
                        Mapper.Map<Person, PersonModel>));
        }
        #endregion

        #region Project
        [GET("projects/{projectid}/people")]
        public IEnumerable<PersonModel> GetProjectPeople(int projectid)
        {
            var project = DbSession
                 .Include<Project>(p => p.People)
                 .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<PersonModel>(
                    DbSession.Load<Person>(project.People).Select(
                        Mapper.Map<Person, PersonModel>));
        }

        [POST("projects/{projectid}/people/{name}")]
        public HttpResponseMessage<PersonModel> PostProjectPeople(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            string name)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = Get<Project>(projectid);

            // todo check if user exists
            var person = Get<Person>("people/" + name);

            project.People.Add(person.Id);
            //todo permissions?
            return new HttpResponseMessage<PersonModel>(Mapper.Map<Person, PersonModel>(person),
                                                        HttpStatusCode.Created);
        }
        #endregion

        #region Task
        [GET("task/{taskid}/people")]
        public IEnumerable<PersonModel> GetTaskPeople(int projectid, int taskid)
        {
            var project = DbSession
                            .Include<Project>(p => p.Tasks)
                            .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Task>(t => t.People)
                .Load<Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<PersonModel>(
                    DbSession.Load<Person>(task.People).Select(
                        Mapper.Map<Person, PersonModel>));
        }

        [POST("task/{taskid}/people/{name}")]
        public HttpResponseMessage<PersonModel> PostTaskPeople(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            string name)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = Get<Task>(taskid);

            var person = Get<Person>("people/"+name);
            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.People.Add(person.Id);
            //todo permissions?
            return new HttpResponseMessage<PersonModel>(Mapper.Map<Person, PersonModel>(person),
                                                        HttpStatusCode.Created);
        }
        #endregion
    }
}