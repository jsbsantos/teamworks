using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Activity = Teamworks.Web.Models.Api.Activity;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Web
{
    public class TimelogController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int projectid, int taskid)
        {
            var project = DbSession
                .Include<Project>(p => p.Activities)
                .Load<Project>(projectid);

            if (project == null)
            {
                return new HttpNotFoundResult();
            }

            if (project.Activities.Count(t => t.Identifier() == taskid) == 0)
            {
                return new HttpNotFoundResult();
            }

            var task = DbSession.Load<Core.Activity>(taskid);
            if (task == null)
            {
                return new HttpNotFoundResult();
            }

            return View(Mapper.Map<Core.Activity, Activity>(task));
        }
    }
}