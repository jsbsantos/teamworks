using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Client;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

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
        public IEnumerable<Person> Get(string q)
        {
            IList<Core.Person> people;
            if (string.IsNullOrEmpty(q))
            {
                people = DbSession.Query<Core.Person>()
                    .Take(5).ToList();
            }
            else
            {
                people = DbSession.Advanced.LuceneQuery<Core.Person>()
                    .Search("Name", q + "*").Search("Username", q + "*")
                    .Search("Email", q + "*").Take(5)
                    .ToList();
            }
            return Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(people.ToList());
        }

        [SecureFor]
        [GET("activities")]
        public IEnumerable<Activity> GetActivities()
        {
            var current = Request.GetCurrentPersonId();
            Request.ThrowNotFoundIfNull(current);
            var activities = DbSession
                .Query<Core.Activity>();

            return Mapper.Map<IList<Core.Activity>, IEnumerable<Activity>>(activities.ToList());
        }

        [SecureFor]
        [GET("time")]
        public IEnumerable<Timelog> GetTime()
        {
            return null;
        }
    }
}