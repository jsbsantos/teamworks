using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Web
{
    public class HomeController : Controller
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }
    }
}