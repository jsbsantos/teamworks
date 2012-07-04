using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    public class PeopleController : RavenApiController
    {
        [GET("people")]
        public IEnumerable<Person> Get()
        {
            return new List<Person>(
                    DbSession.Query<Core.Person>().Select(
                        Mapper.Map<Core.Person, Person>));
        }
    }
}