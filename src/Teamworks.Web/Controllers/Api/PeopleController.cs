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
using Person = Teamworks.Web.Models.Person;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class PeopleController : RavenApiController
    {
        #region General
        //todo unbound (limited by raven) result set - page results?
        [GET("people")]
        public IEnumerable<Person> GetPeople()
        {
            return new List<Person>(
                    DbSession.Query<Core.Person>().Select(
                        Mapper.Map<Core.Person, Person>));
        }
        #endregion

        #region Project
        [GET("projects/{projectid}/people")]
        public IEnumerable<Person> GetProjectPeople(int projectid)
        {
            var project = DbSession
                 .Include<Project>(p => p.People)
                 .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<Person>(
                    DbSession.Load<Core.Person>(project.People).Select(
                        Mapper.Map<Core.Person, Person>));
        }

        [POST("projects/{projectid}/people/{name}")]
        public HttpResponseMessage<Person> PostProjectPeople(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            string name)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = Get<Project>(projectid);

            // todo check if user exists
            var person = Get<Core.Person>("people/" + name);

            project.People.Add(person.Id);
            //todo permissions?
            return new HttpResponseMessage<Person>(Mapper.Map<Core.Person, Person>(person),
                                                        HttpStatusCode.Created);
        }
        #endregion

        #region Task
        [GET("task/{taskid}/people")]
        public IEnumerable<Person> GetTaskPeople(int projectid, int taskid)
        {
            var project = DbSession
                            .Include<Project>(p => p.Tasks)
                            .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var task = DbSession
                .Include<Core.Task>(t => t.People)
                .Load<Core.Task>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new List<Person>(
                    DbSession.Load<Core.Person>(task.People).Select(
                        Mapper.Map<Core.Person, Person>));
        }

        [POST("task/{taskid}/people/{name}")]
        public HttpResponseMessage<Person> PostTaskPeople(
            [ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
            [ModelBinder(typeof (TypeConverterModelBinder))] int taskid,
            string name)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = Get<Core.Task>(taskid);

            var person = Get<Core.Person>("people/"+name);
            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            task.People.Add(person.Id);
            //todo permissions?
            return new HttpResponseMessage<Person>(Mapper.Map<Core.Person, Person>(person),
                                                        HttpStatusCode.Created);
        }
        #endregion
    }
}