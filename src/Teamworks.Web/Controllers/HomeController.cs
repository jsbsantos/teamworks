using System.Web.Mvc;
using Raven.Client.Document;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Models;

namespace TeamWorks.Web.Controllers {
    public class HomeController : Controller {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index() {
            return View(ProjectsController.Projects);
        }

        [HttpGet]
        public ActionResult Raven() {
            using (var store = new DocumentStore() {ConnectionStringName = "RavenDB"}.Initialize()) {
                using (var session = store.OpenSession()) {
                    session.Store(new Project()
                                  {
                                      Name = "teamworks",
                                      Description = "Track your projects everywhere"
                                  });
                    session.SaveChanges();
                }
            }
            return new ContentResult() {Content = "Project added =)."};
        }
    }
}