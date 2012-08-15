using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ErrorsController : Controller
    {
        [HttpGet]
        public ActionResult NoDb()
        {
            return View();
        }

    }
}
