using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Web
{
    public class TimelineController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int id)
        {
            return new ContentResult {Content = "Not Implemented."};
        }
    }
}