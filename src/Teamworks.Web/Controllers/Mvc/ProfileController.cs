using System.Web.Mvc;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProfileController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            ViewBag.Me = true;
            return View(((PersonIdentity) User.Identity).Person);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View("View", ((PersonIdentity)User.Identity).Person);
        }

        [HttpPost]
        public ActionResult Edit(string name)
        {
            return RedirectToAction("Edit");
        }
    }
}
