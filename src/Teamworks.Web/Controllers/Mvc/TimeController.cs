using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class TimeController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            var activities = DbSession.Query<Core.Activity>()
                .Customize(c => c.Include<Core.Activity>(a => a.Project))
                .ToList();


            var source = new List<object>();
            var timelogs = new List<object>();
            foreach (var activity in activities)
            {
                var project = DbSession.Load<Core.Project>(activity.Project);
                source.Add(new
                               {
                                   Activity = new
                                                  {
                                                      Id = activity.Identifier,
                                                      activity.Name
                                                  },
                                   Project = new
                                                 {
                                                     Id = project.Identifier,
                                                     project.Name
                                                 }
                               });

                foreach (var t in activity.Timelogs)
                {
                    timelogs.Add(new
                                     {
                                         t.Duration,
                                         Activity = new
                                                        {
                                                            Id = activity.Identifier,
                                                            activity.Name
                                                        },
                                         Project = new
                                                       {
                                                           Id = project.Identifier,
                                                           project.Name
                                                       },
                                         t.Description,
                                         t.Date
                                     });
                }
            }

            dynamic model = new ExpandoObject();
            model.Source = source;
            model.Timelogs = timelogs;

            return View(model);
        }
    }
}