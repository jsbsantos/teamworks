using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    [DefaultHttpRouteConvention]
    public class PeopleController : RavenDbApiController
    {
        [GET("people")]
        public IEnumerable<string> Get(string q)
        {
            if (string.IsNullOrEmpty(q)) {
               return DbSession.Query<Core.Person>()
                   .Take(5).Select(p => p.Email).ToList();    
            }
            return DbSession.Advanced.LuceneQuery<Core.Person>()
                    .Search("Email", q + "*").Take(5).Select(p=>p.Email)
                    .ToList();
            //return Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(people);
        }

        [GET("activities")]
        [SecureFor("/projects")]
        public IEnumerable<Activity> GetActivities()
        {
            var projects = DbSession.Query<Core.Project>()
                .Customize(q => q.Include<Core.Project>(p => p.Activities));

            var activities = DbSession.Load<Core.Activity>(projects.ToList().SelectMany(p => p.Activities).Distinct()).ToList();
            return Mapper.Map<IList<Core.Activity>, IEnumerable<Activity>>(activities);
        } 

        
    }
}