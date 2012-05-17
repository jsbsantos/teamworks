using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Controllers
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
                var _task = DbSession
                    .Include("TaskModel.ProjectId")
                    .Load<Task>(id);
                    
                if (_task == null || (_task != null && _task.ProjectId != projectid))
                    throw new HttpException(404, "Not Found");

                Models.ProjectModel proj = Mapper.Map<Project, Models.ProjectModel>(DbSession.Load<Project>(_task.ProjectId));
                Models.TaskModel taskModel = Mapper.Map<Task, Models.TaskModel>(_task);

                return View("Task", new {proj, task = taskModel});
            }

            var project = DbSession.Load<Project>("projects/" + (string.IsNullOrEmpty(projectid) ? "-1" : projectid));
            if (project == null)
                throw new HttpException(404, "Not Found");

            return View(/*Mapper.Map<Project, Models.Project>(project)*/);
        }

    }
}
