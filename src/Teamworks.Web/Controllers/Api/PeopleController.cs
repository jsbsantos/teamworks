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
        public IEnumerable<string> Get(string filter)
        {
            IList < Core.Person > people;
            if (string.IsNullOrEmpty(filter)) {
               people = DbSession.Query<Core.Person>().ToList();    
            }

            people = DbSession.Advanced.LuceneQuery<Core.Person>()
                .Search("Name", "*" + filter + "*")
                .Search("Username", "*" + filter + "*")
                .Take(5).ToList();

            return people.Select(p => p.Username);
        }
    }
}