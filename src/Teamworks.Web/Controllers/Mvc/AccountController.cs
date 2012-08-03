using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using Raven.Json.Linq;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Oauth2;
using Teamworks.Web.Models.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [AllowAnonymous]
    public class AccountController : RavenController
    {
        private const string ReturnUrlKey = "RETURN_URL_KEY";

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return View("View");
            }
            Session[ReturnUrlKey] = returnUrl;
            return RedirectToAction("Login");
        }
        
        [HttpGet]
        public ActionResult OpenId(string provider)
        {
            var result = new OpenId().Authenticate(provider); // first request, set state to 0 (zero)

            if (result.State < 0) // opendid auth response with error
            {
                ModelState.AddModelError("", "Authentication failed. Correct errors and try again.");
                return View("View");
            }
            if (result.State > 0) // opendid auth response with success
            {
                var username = result.First + result.Last;
                var person = GetUser(username, result.Email);
                if (person == null)
                {
                    person = Core.Person.Forge(result.Email, username, null, string.Format("{0} {1}", result.First, result.Last));
                    DbSession.Store(person);
                    SetOpenId(DbSession, person, provider, result.ClaimedIdentifier);
                }
                return SetUpAuthenticatedUser(person, true);
            }
            return new EmptyResult();
        }
        
        private static void SetOpenId(IDocumentSession session, Core.Person person, string provider, string claim)
        {
            var metadata = session.Advanced.GetMetadataFor(person);
            metadata[Core.Oauth2.OpenId.ProviderKey] = provider;
            metadata[Core.Oauth2.OpenId.ClaimKey] = claim;
        }

        private static string GetOpenIdProvider(IDocumentSession session, Core.Person person)
        {
            var metadata = session.Advanced.GetMetadataFor(person);
            return metadata[Core.Oauth2.OpenId.ProviderKey].Value<string>();
        }

        private static string GetOpenIdClaim(IDocumentSession session, Core.Person person)
        {
            var metadata = session.Advanced.GetMetadataFor(person);
            return metadata[Core.Oauth2.OpenId.ClaimKey].Value<string>();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View("View", model);
            }

            var person = DbSession.Query<Person>().SingleOrDefault(
                p => p.Username.Equals(model.Username, StringComparison.InvariantCultureIgnoreCase));

            if (person != null && person.IsThePassword(model.Password))
            {
                return SetUpAuthenticatedUser(person, model.Persist);
            }

            ModelState.AddModelError("", "The username or password you entered is incorrect.");
            return View("View", model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("View", "Home");
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View("View");
        }

        [HttpPost]
        public ActionResult Signup(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View("View");
            }
            var user = GetUser(register.Username, register.Email);
            if (user != null)
            {
                ModelState.AddModelError("model.unique",
                                         "The username or email you specified already exists in the system");
                return RedirectToAction("Signup");
            }
            var person = Core.Person.Forge(register.Email, register.Username, register.Password, register.Name);
            DbSession.Store(person);

            return RedirectToAction("View", "Home");
        }

        private Core.Person GetUser(string username = "", string email = "")
        {
            var list = DbSession.Query<Core.Person>()
                .Where(p => p.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) ||
                            p.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            return list.SingleOrDefault();
        }

        private ActionResult SetUpAuthenticatedUser(Core.Person person, bool persist)
        {
            var returnUrl = Session[ReturnUrlKey] as string
                            ?? FormsAuthentication.DefaultUrl;

            FormsAuthentication.SetAuthCookie(person.Id, persist);
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("View", "Home");
        }

        #region OAuth

        [HttpGet]
        public ActionResult SignupOAuth(string provider)
        {
            var p = new Google()
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
            var provider = (Google) Session["teamworks.oauth.provider"];

            var content = JObject.Parse(provider.GetProfile(Request.QueryString["code"]));
            var person = Core.Person.Forge(content.Value<string>("email"),
                                           content.Value<string>("name"), null, content.Value<string>("name"));

            var personExists =
                DbSession.Query<Core.Person>().Where(
                    p => p.Username.Equals(person.Username, StringComparison.InvariantCultureIgnoreCase) ||
                         p.Email.Equals(person.Email, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

            if (personExists.Count > 0)
            {
                ModelState.AddModelError("model.unique",
                                         "The username or email you specified already exists in the system");
                return RedirectToAction("Signup");
            }

            DbSession.Store(person);
            return RedirectToAction("View", "Home");
        }

        #endregion

    }
}