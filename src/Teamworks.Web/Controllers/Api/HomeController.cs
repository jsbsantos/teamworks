using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class HomeController : RavenDbApiController
    {

        [SecureFor]
        [GET("activities")]
        public IEnumerable<Activity> Activities()
        {
            var current = Request.GetCurrentPersonId();
            var activities = DbSession
                .Query<Core.Activity>()
                .Where(a => a.People.Contains(current));

            return Mapper.Map<IList<Core.Activity>, IEnumerable<Activity>>(activities.ToList());
        }

        [SecureFor]
        [GET("time")]
        public IEnumerable<Timelog> Timelogs()
        {
            return null;
        } 
    }
}