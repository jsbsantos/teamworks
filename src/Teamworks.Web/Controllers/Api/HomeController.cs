using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class HomeController : RavenApiController
    {
        public HomeController()
        {
        }

        public HomeController(IDocumentSession session)
            : base(session)
        {
        }

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