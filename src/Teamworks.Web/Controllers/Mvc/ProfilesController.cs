using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProfilesController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                return View(DbSession.Load<Person>(id.Value));
            }
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
