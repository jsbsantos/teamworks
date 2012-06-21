using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Project = Teamworks.Core.Project;
using Task = Teamworks.Web.Models.Task;

namespace Teamworks.Web.Controllers.Web
{
    public class TasksController : RavenController
    {
        //route: projects/{projectid}/tasks/{id}
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id, int projectid)
        {
            int taskid;
            
            var project = DbSession
                .Include<Project>(p => p.Tasks)
                .Load<Project>(projectid);

            if (project == null)
            {
                return new HttpNotFoundResult();
            }

            bool parse = int.TryParse(id, out taskid);
            if (id != null && parse)
            {
                if (project.Tasks.Count(t => t.Identifier() == taskid) == 0)
                {
                    return new HttpNotFoundResult();
                }
                var task = DbSession.Load<Core.Task>(taskid);
                Task model = Mapper.Map<Core.Task, Task>(task);
                return View("Task", model);
            }

            return View(Mapper.Map<List<Core.Task>, List<Task>>(DbSession.Load<Core.Task>(project.Tasks).ToList()));
        }
    }
}