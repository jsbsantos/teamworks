using System.Web.Mvc;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProfileController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier)
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
        public ActionResult Edit(object model)
        {
            return RedirectToAction("Edit");
        }
    }
}
