using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Web
{
    public class TimelogController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int projectid, int taskid)
        {
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                return new HttpNotFoundResult();
            }

            if (project.Tasks.Count(t => t.Identifier() == taskid) == 0)
            {
                return new HttpNotFoundResult();
            }

            var task = DbSession.Load<Task>(taskid);
            if (task == null)
            {
                return new HttpNotFoundResult();
            }

            return View(Mapper.Map<Task, TaskModel>(task));
        }
    }
}