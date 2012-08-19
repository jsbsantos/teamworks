using System.Web.Mvc;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Controllers.Mvc
{
    [AllowAnonymous]
    public class PasswordController : RavenController
    {
        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(string login)
        {
            var person = DbSession.GetPersonByLogin(login ?? "");
            if (person == null)
                ModelState.AddModelError("model.login", "We could not find any user with the username/email specified.");

            if (!ModelState.IsValid)
                return View();

            // todo send email with token
            return RedirectToAction("Change");
        }

        [HttpGet]
        public ActionResult Change(string token)
        {
            return View((object) token);
        }

        [HttpPost]
        public ActionResult Change(string token, string password, string passwordConfirm)
        {
            return RedirectToRoute("homepage");
        }

    }
}
