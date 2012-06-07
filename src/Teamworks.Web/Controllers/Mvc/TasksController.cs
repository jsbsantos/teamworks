using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;

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
                var task = DbSession.Load<Task>(taskid);
                TaskModel model = Mapper.Map<Task, TaskModel>(task);
                return View("Task", model);
            }

            return View(Mapper.Map<List<Task>, List<TaskModel>>(DbSession.Load<Task>(project.Tasks).ToList()));
        }
    }
}