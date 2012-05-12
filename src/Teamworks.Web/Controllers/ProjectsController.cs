using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Controllers {
    [Authorize]
    public class ProjectsController : RavenController {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id) {
            if (id != null) {
                var proj = DbSession.Load<Project>(id);
                if (proj == null) {
                    throw new HttpException(404, "Not Found");
                }

                Models.Project project = Mapper.Map<Project, Models.Project>(proj);
                return View("Project", project);
            }
            return View();
        }
    }
}