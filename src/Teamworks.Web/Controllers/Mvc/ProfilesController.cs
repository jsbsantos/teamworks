using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;
using Teamworks.Web.Views;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProfilesController : RavenController
    {
        public ActionResult Index(int? id)
        {
            var personViewModel = new ProfileViewModel();

            var person = id.HasValue
                             ? DbSession.Load<Person>(id)
                             : HttpContext.GetCurrentPerson();

            if (person.Id == HttpContext.GetCurrentPersonId())
                personViewModel.IsMyProfile = true;

            personViewModel.PersonDetails = person.MapTo<PersonViewModel>();
            return View(personViewModel);
        }

        public ActionResult Edit(ProfileViewModel.Input model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var person = HttpContext.GetCurrentPerson();
            model.MapPropertiesToInstance(person);

            if (Request.IsAjaxRequest())
            {
                return new JsonNetResult {
                               Data = person.MapTo<PersonViewModel>()
                           };
            }
            return RedirectToAction("Index");
        }
    }
}