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
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    public class PeopleController : RavenApiController
    {
        #region General
        //todo unbound (limited by raven) result set - page results?
        [GET("api/people")]
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
                 .Include<Core.Project>(p => p.People)
                 .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new List<Person>(
                    DbSession.Load<Core.Person>(project.People).Select(
                        Mapper.Map<Core.Person, Person>));
        }

        [POST("projects/{projectid}/people/{name}")]
        public HttpResponseMessage PostProjectPeople(
             int projectid,
            string name)
        {
            var project = DbSession.Load<Core.Project>(projectid);
            var person = DbSession.Load<Core.Person>("people/" + name);

            project.People.Add(person.Id);
            return Request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Core.Person, Person>(person));
        }
        #endregion

        #region Activities
        [GET("task/{taskid}/people")]
        public IEnumerable<Person> GetTaskPeople(int projectid, int taskid)
        {
            var project = DbSession
                            .Include<Core.Project>(p => p.Activities)
                            .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var task = DbSession
                .Include<Core.Activity>(t => t.People)
                .Load<Core.Activity>(taskid);

            if (task == null || !task.Project.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new List<Person>(
                    DbSession.Load<Core.Person>(task.People).Select(
                        Mapper.Map<Core.Person, Person>));
        }

        [POST("task/{taskid}/people/{name}")]
        public HttpResponseMessage PostTaskPeople(
             int projectid,
             int taskid,
            string name)
        {
            var task = DbSession.Load<Core.Activity>(taskid);

            var person = DbSession.Load<Core.Person>("people/"+name);
            task.People.Add(person.Id);

            var value = Mapper.Map<Core.Person, Person>(person);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }
        #endregion
    }
}