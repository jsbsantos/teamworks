using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProfilesController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? id)
        {
            PersonViewModel personViewModel;
            if (id.HasValue)
            {
                personViewModel = DbSession.Load<Person>(id.Value)
                    .MapTo<PersonViewModel>();
            }
            else
            {
                ViewBag.Me = true;
                personViewModel = ((PersonIdentity) User.Identity).Person
                    .MapTo<PersonViewModel>();
            }
            return View(personViewModel);
        }

        /*
        [HttpGet]
        public ActionResult Edit()
        {
            return View("View", ((PersonIdentity) User.Identity).Person);
        }

        [HttpPost]
        public ActionResult Edit(object model)
        {
            return RedirectToAction("Edit");
        }
        */
    }
}