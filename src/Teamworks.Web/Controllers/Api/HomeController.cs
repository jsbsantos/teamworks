using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class HomeController : RavenApiController
    {
        [GET("people")]
        public IEnumerable<PersonViewModel> Get(string q)
        {
            IList<Person> people;
            if (string.IsNullOrEmpty(q))
            {
                people = DbSession.Query<Person>()
                    .Take(5).ToList();
            }
            else
            {
                people = DbSession.Advanced.LuceneQuery<Person>()
                    .Search("Name", q + "*").Search("Username", q + "*")
                    .Search("Email", q + "*").Take(5)
                    .ToList();
            }
            return people.MapTo<PersonViewModel>();
        }


    }
}