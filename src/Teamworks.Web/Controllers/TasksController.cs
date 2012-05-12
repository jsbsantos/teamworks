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
                    .Include("Task.ProjectId")
                    .Load<Task>(id);
                    
                if (_task == null || (_task != null && _task.ProjectId != projectid))
                    throw new HttpException(404, "Not Found");

                Models.Project proj = Mapper.Map<Project, Models.Project>(DbSession.Load<Project>(_task.ProjectId));
                Models.Task task = Mapper.Map<Task, Models.Task>(_task);

                return View("Task", new {proj, task});
            }

            var project = DbSession.Load<Project>("projects/"+projectid ?? "-1");
            if (project == null)
                throw new HttpException(404, "Not Found");

            return View(Mapper.Map<Project, Models.Project>(project));
        }

    }
}
