using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Web
{
    public class TasksController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id, int projectid)
        {
            if (id != null)
            {
                var task = DbSession
                    .Include("TaskModel.ProjectId")
                    .Load<Task>(id);

                if (task == null || (task != null && !task.ProjectId.Contains(projectid.ToString())))
                    throw new HttpException(404, "Not Found");

                ProjectModel proj = Mapper.Map<Project, ProjectModel>(DbSession.Load<Project>(task.ProjectId));
                TaskModel taskModel = Mapper.Map<Task, TaskModel>(task);

                return View("Task", new {proj, task = taskModel});
            }

            var project = DbSession.Load<Project>("projects/" + projectid);
            if (project == null)
                throw new HttpException(404, "Not Found");

            return View( /*Mapper.Map<Project, Models.Project>(project)*/);
        }
    }
}