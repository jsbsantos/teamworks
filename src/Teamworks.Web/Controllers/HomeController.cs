using System.Web.Mvc;
using Raven.Client.Document;
using Teamworks.Web.Controllers;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Models;

namespace TeamWorks.Web.Controllers
{
    public class HomeController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View(ProjectsController.Projects);
        }

        [HttpGet]
        public ActionResult Raven()
        {
            RavenSession.Store(new Project()
                              {
                                  Name = "teamworks",
                                  Description = "Track your projects everywhere"
                              });
            RavenSession.SaveChanges();
            return new ContentResult() { Content = "Project added =)." };
        }
    }
}