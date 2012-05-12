using System.Web.Mvc;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Controllers
{
    public class TimelineController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int id) {
            return new ContentResult() { Content = "Not Implemented." };
        }
    }
}