using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Web.Models.Task;

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

            var task = DbSession.Load<Core.Task>(taskid);
            if (task == null)
            {
                return new HttpNotFoundResult();
            }

            return View(Mapper.Map<Core.Task, Task>(task));
        }
    }
}