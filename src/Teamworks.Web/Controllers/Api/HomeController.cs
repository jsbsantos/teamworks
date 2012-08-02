using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class HomeController : RavenDbApiController
    {

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