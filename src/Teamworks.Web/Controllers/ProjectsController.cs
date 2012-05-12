using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Controllers {
    public class ProjectsController : RavenController {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id) {
            if (id != null) {
                var project = DbSession.Load<Project>(id);
                if (project == null) {
                    throw new HttpException(404, "Not Found");
                }

                var ret = Mapper.Map<Project, Models.Project>(project);
                return View("Project", ret);
            }
            return View();
        }
    }
}