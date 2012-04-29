using System.Web.Mvc;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers
{
    public class ProjectsController : Controller
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int id)
        {
            return View(new Project
                        {
                            Id = id,
                            Name = "Project",
                            Description = "Project description"
                        });
        }

    }
}
