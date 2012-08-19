using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json.Linq;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Oauth2;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [AllowAnonymous]
    public class AccountController : RavenController
    {
        private const string ReturnUrlKey = "RETURN_URL_KEY";

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated)
                RedirectFromLoginPage();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Session[ReturnUrlKey] = returnUrl;
                return RedirectToAction("Index");
            }

            return View(new AccountViewModel());
        }

        [HttpPost]
        public ActionResult Index(AccountViewModel input)
        {
            var person = DbSession.GetPersonByLogin(input.Username);

            if (person == null || !person.IsThePassword(input.Password))
            {
                ModelState.AddModelError("UserNotExistOrPasswordNotMatch",
                    "Username/Email and password do not match any known user.");  
            } 

            if (ModelState.IsValid)
            {
                var persist = input.Persist && input.Persist;
                FormsAuthentication.SetAuthCookie(person.Id, persist);
                return RedirectFromLoginPage();
            }

            return View(new AccountViewModel() { Username = input.Username });
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Register(AccountViewModel.Register model)
        {
            var email = DbSession.GetPersonByEmail(model.Email);
            var username = DbSession.GetPersonByUsername(model.Username);

            if (email != null)
            {
                ModelState.AddModelError("EmailAlreadyInUse",
                    "The email must be unique.");
            }

            if (username != null)
            {
                ModelState.AddModelError("UsernameAlreadyInUse",
                    "The username must be unique.");
            }

            if (ModelState.IsValid)
            {
                var person = Person.Forge(model.Email, model.Username, model.Password, model.Name);
                DbSession.Store(person);
                
                FormsAuthentication.SetAuthCookie(person.Id, false);
                return RedirectFromLoginPage();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return RedirectFromLoginPage();
        }

        [HttpGet]
        public ActionResult OpenId(string provider, string returnUrl)
        {
            var result = new OpenId()
                .Authenticate(provider); // first request, set state to 0 (zero)

            if (result.State < 0) // opendid auth response with error
            {
                ModelState.AddModelError("", "Authentication failed. Correct errors and try again.");
                return View("Index", new AccountViewModel());
            }
            if (result.State > 0) // opendid auth response with success
            {
                var person = DbSession.GetPersonByEmail(result.Email);
                if (person == null)
                {
                    var username = result.First + result.Last;
                    var name = string.Format("{0} {1}", result.First, result.Last);
                    person = Person.Forge(result.Email, username, "", name);
                    DbSession.Store(person);

                    var metadata = DbSession.Advanced.GetMetadataFor(person);
                    metadata[Core.Oauth2.OpenId.ProviderKey] = provider;
                    metadata[Core.Oauth2.OpenId.ClaimKey] = result.ClaimedIdentifier;
                }
                FormsAuthentication.SetAuthCookie(person.Id, false);
                return RedirectFromLoginPage(returnUrl);
            }
            return new EmptyResult();
        }

        private ActionResult RedirectFromLoginPage(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Session[ReturnUrlKey] as string;

            Session.Remove(ReturnUrlKey);
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToRoute("homepage");
                
            return Redirect(returnUrl);
        }

        #region OAuth

        [HttpGet]
        public ActionResult SignupOAuth(string provider)
        {
            var p = new Google
            {
                ClientId = @"937363753546.apps.googleusercontent.com",
                Secret = "lcUNnsobBB5sl_1NHohHnhlh",
                Callback = "http://alkalined.dyndns.org/account/GoogleOAuth"
            };
            Session["teamworks.oauth.provider"] = p;
            return Redirect(p.Url);
        }

        [HttpGet]
        public ActionResult GoogleOAuth(string code)
        {
            var provider = (Google)Session["teamworks.oauth.provider"];

            JObject content = JObject.Parse(provider.GetProfile(Request.QueryString["code"]));
            Person person = Person.Forge(content.Value<string>("email"),
                                         content.Value<string>("name"), null, content.Value<string>("name"));

            List<Person> personExists =
                DbSession.Query<Person>().Where(
                    p => p.Username.Equals(person.Username, StringComparison.InvariantCultureIgnoreCase) ||
                         p.Email.Equals(person.Email, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

            if (personExists.Count > 0)
            {
                ModelState.AddModelError("model.unique",
                                         "The username or email you specified already exists in the system");
                return RedirectToAction("Register");
            }

            DbSession.Store(person);
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }

    
}