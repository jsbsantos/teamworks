using System;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Core.Mailgun;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [AllowAnonymous]
    public class PasswordController : AppController
    {
        private const string ResetTokenKey = "Reset-Token";
        private const string ResetTokenDateKey = "Reset-Token-Expiration";

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

            var tk = Utils.Token();
            
            DbSession.Advanced.GetMetadataFor(person)[ResetTokenKey] = tk;
            DbSession.Advanced.GetMetadataFor(person)[ResetTokenDateKey] = DateTimeOffset.UtcNow.AddDays(1);

            var link = Url.RouteUrl("password_change",
                new {username = person.Username, token = tk},
                Request.Url.Scheme);

            MailHub.NoReply(person.Email, "Reset your password", "Reset your password on Teamworks clicking here " + link);
            return RedirectToRoute("homepage");
        }

        [GET("password/{username}/{token}")]
        public ActionResult Change(string username, string token)
        {
            var person = DbSession.GetPersonByUsername(username);
            if(!IsTokenValid(person, token))
                return View("Expired");
            
            return View(new PasswordViewModel { Username = username, Token = token});
        }

        [POST("password/{username}/{token}")]
        public ActionResult PostChange(string username, string token, string password, string passwordConfirm)
        {
            if (password != passwordConfirm)
                ModelState.AddModelError("model.password", "Password and confirmation must match");
            
            if (!ModelState.IsValid)
                return View("Change");

            var person = DbSession.GetPersonByUsername(username);
            if (!IsTokenValid(person, token))
                return View("Expired");

            DbSession.Advanced.GetMetadataFor(person).Remove(ResetTokenKey);
            DbSession.Advanced.GetMetadataFor(person).Remove(ResetTokenDateKey);

            person.ChangePassword(password);
            return RedirectToRoute("homepage");
        }

        [NonAction]
        public bool IsTokenValid(Person person, string token)
        {
            if (person == null)
                return false;

            var tk = DbSession.Advanced
                .GetMetadataFor(person).Value<string>(ResetTokenKey);

            var str = DbSession.Advanced
                .GetMetadataFor(person).Value<string>(ResetTokenDateKey);

            var date = DateTimeOffset.Parse(str);
            return tk == token && DateTimeOffset.UtcNow.CompareTo(date) < 0;
        }

    }

    public class PasswordViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
