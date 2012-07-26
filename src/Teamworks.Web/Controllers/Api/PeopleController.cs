using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/people")]
    [DefaultHttpRouteConvention]
    public class PeopleController : RavenApiController
    {
        public IEnumerable<Person> Get(string q)
        {
            IList < Core.Person > people;
            if (string.IsNullOrEmpty(q)) {
               people = DbSession.Query<Core.Person>().ToList();    
            }

            people = DbSession.Advanced.LuceneQuery<Core.Person>()
                .Search("Name", q + "*")
                .Search("Username", q + "*")
                .Take(5).ToList();

            return Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(people);
        }
    }
}