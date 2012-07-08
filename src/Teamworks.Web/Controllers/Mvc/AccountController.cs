using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using Raven.Client.Linq;
using Teamworks.Core.Authentication;
using Teamworks.Core.Oauth2;
using Teamworks.Core.Services;
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
                return View();
            }
            Session[ReturnUrlKey] = returnUrl;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult LoginOpenID(string returnUrl, string provider)
        {
            if (string.IsNullOrEmpty(provider))
            {
                return View("Login");
            }

            //Session[ReturnUrlKey] = returnUrl;

            var result = new OpenId().Authenticate(provider);

            return OpenIdHandler(
                result,
                () =>
                    {
                        ModelState.AddModelError("", "The username or password you entered is incorrect.");
                        return View("login");
                    },
                () =>
                    {
                        var url = Session[ReturnUrlKey] as string
                                  ?? FormsAuthentication.DefaultUrl;

                        var person = DbSession.Query<Core.Person>()
                            .Where(p => p.Email.Equals(result.Email)).SingleOrDefault();

                        if (person == null)
                        {
                            ModelState.AddModelError("", "Invalid user or password.");
                            return RedirectToAction("Signup", "Account");
                        }

                        return SetUpAuthenticatedUser(person, true);
                    }
                );
        }


        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            dynamic dyn = new ExpandoObject();
            dyn.Username = model.Username;
            dyn.Password = model.Password;

            Core.Person person;
            IAuthenticator auth = Global.Authentication["Basic"];
            if (auth.IsValid(dyn, out person))
            {
                return SetUpAuthenticatedUser(person, model.Persist);
            }

            ModelState.AddModelError("", "The username or password you entered is incorrect.");
            return View(model);
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
            return View();
        }

        [HttpGet]
        public ActionResult SignupOpenId(string provider)
        {
            var result = new OpenId().Authenticate(provider);

            return OpenIdHandler(
                result,
                () =>
                    {
                        ModelState.AddModelError("", "Authentication failed. Correct errors and try again.");
                        return View("Signup");
                    },
                () => CreateUser(result.First + result.Last, null, result.Email)
                          ? RedirectToAction("View", "Home")
                          : RedirectToAction("Signup"));
        }

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
                                           content.Value<string>("name"), null);

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

        [HttpPost]
        public ActionResult Signup(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return CreateUser(register.Username, register.Password, register.Email)
                       ? RedirectToAction("View", "Home")
                       : RedirectToAction("Signup");
        }

        private ActionResult OpenIdHandler(dynamic result,
                                           Func<ActionResult> fail,
                                           Func<ActionResult> success)
        {
            switch ((int) (result.State))
            {
                case -1:
                    return fail();
                case 0:
                    return new EmptyResult();
                case 1:
                    return success();
                default:
                    throw new NotSupportedException();
            }
        }

        private bool CreateUser(string username, string password, string email)
        {
            var personExists =
                DbSession.Query<Core.Person>().Where(
                    p => p.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) ||
                         p.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

            if (personExists.Count > 0)
            {
                ModelState.AddModelError("model.unique",
                                         "The username or email you specified already exists in the system");
                return false;
            }

            var person = Core.Person.Forge(email, username, password);

            DbSession.Store(person);
            return true;
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
    }
}