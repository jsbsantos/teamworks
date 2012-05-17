using System.Collections.Generic;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core.People;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectid}")]
    public class PeopleController : RavenApiController
    {
        [GET("people")]
        public IEnumerable<Person> Get(string projectid)
        {
            return new List<Person> {Person.Forge("a@b.c", "user", "password")};
        }

        [GET("tasks/{taskid}/people")]
        public IEnumerable<Person> Get(string projectid, string taskid)
        {
            return new List<Person> {Person.Forge("a@b.c", "user", "password")};
        }
    }
}