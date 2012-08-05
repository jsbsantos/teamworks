using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Mvc.Timelogs;

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

            
            var source = new List<TimeTypeahead>();
            var timelogs = new List<Timelog>();
            foreach (var a in activities)
            {
                var project = DbSession.Load<Core.Project>(a.Project);
                source.Add( new TimeTypeahead
                                     {
                                         Activity = a.Name,
                                         ActivityId = a.Identifier,
                                         Project = project.Name,
                                         ProjectId = project.Identifier
                                     });

                foreach (var t in a.Timelogs)
                {
                    var timelog =  Mapper.Map<Core.Timelog, Timelog>(t);
                    timelog.Activity = a;
                    timelog.Project = project;
                    timelogs.Add(timelog);
                }
            }

            var model = new TimeViewModel {
                                Source = source,
                                Timelogs = timelogs
                            };

            return View(model);
        }
    }

}