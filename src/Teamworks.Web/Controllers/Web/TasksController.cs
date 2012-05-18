using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Web
{
    [RoutePrefix("projects/{projectid}")]
    public class TasksController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        [GET("tasks/{id}")]
        public ActionResult Index(string id, string projectid)
        {
            if (id != null)
            {
                var task = DbSession
                    .Include("TaskModel.ProjectId")
                    .Load<Task>(id);

                if (task == null || (task != null && task.Project != projectid))
                    throw new HttpException(404, "Not Found");

                ProjectModel proj = Mapper.Map<Project, ProjectModel>(DbSession.Load<Project>(task.Project));
                TaskModel taskModel = Mapper.Map<Task, TaskModel>(task);

                return View("Task", new {proj, task = taskModel});
            }

            var project = DbSession.Load<Project>("projects/" + (string.IsNullOrEmpty(projectid) ? "-1" : projectid));
            if (project == null)
                throw new HttpException(404, "Not Found");

            return View( /*Mapper.Map<Project, Models.Project>(project)*/);
        }
    }
}