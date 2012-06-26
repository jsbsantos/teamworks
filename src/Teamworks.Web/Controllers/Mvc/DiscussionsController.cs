using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class DiscussionsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier, int projectid)
        {
            string model = string.Format("/api/projects/{0}/discussions/", projectid);
            return identifier != null
                       ? View("Discussion", (object) (model + identifier.Value))
                       : View("Discussions", (object) model);
        }
    }
}