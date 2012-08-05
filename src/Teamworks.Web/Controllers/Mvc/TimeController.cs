using System.Linq;
using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Models.Mvc.Timelogs;

namespace Teamworks.Web.Controllers.Mvc
{
    public class TimeController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            var activities = DbSession.Query<Activity>()
                .Customize(c => c.Include<Activity>(a => a.Project))
                .ToList();
            
            var source = activities.Select(a =>
                                 new TimeTypeahead
                                     {
                                         Activity = a.Name,
                                         ActivityId = a.Id.ToIdentifier(),
                                         Project =
                                             DbSession.Load<Project>(a.Project).Name,
                                         ProjectId = a.Project.ToIdentifier()
                                     });


            var model = new TimeViewModel {
                                Source = source
                            };

            return View(model);
        }
    }
}