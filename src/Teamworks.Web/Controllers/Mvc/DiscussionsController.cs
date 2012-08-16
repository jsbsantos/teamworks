using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("projects/{projectId}")]
    public class DiscussionsController : RavenController
    {
        [GET("discussions")]
        [ActionName("View")]
        public ActionResult Index(int projectid)
        {
            return View("Discussion");
        }

        [GET("discussions/{discussionId}")]
        public ActionResult Details(int discussionId ,int projectId)
        {
            return View("Discussion");
        }
    }
}