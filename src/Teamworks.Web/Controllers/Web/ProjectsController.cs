using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Web
{
    public class ProjectsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var project = DbSession.Load<Project>(id);
                if (project == null)
                {
                    throw new HttpException(404, "Not Found");
                }

                ProjectModel ret = Mapper.Map<Project, ProjectModel>(project);
                return View("Project", ret);
            }
            var projects = DbSession.Query<Project>().ToList();
            return View(Mapper.Map<List<Project>, IEnumerable<ProjectModel>>(projects));
        }
    }
}