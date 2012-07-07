using System.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
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
                var returnUrl = Session[ReturnUrlKey] as string
                                ?? FormsAuthentication.DefaultUrl;

                FormsAuthentication.SetAuthCookie(person.Id, model.Persist);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("View", "Home");
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
        public ActionResult Signup2(string provider)
        {
            var p = new Oauth()
                        {
                            ClientId = @"937363753546.apps.googleusercontent.com",
                            Secret = "lcUNnsobBB5sl_1NHohHnhlh",
                            Callback = "http://alkalined.dyndns.org/account/oauth"
                        };

            return Redirect(p.Url);
        }

        [HttpGet]
        public ActionResult oauth(string code)
        {
            var p = new Oauth()
                        {
                            ClientId = @"937363753546.apps.googleusercontent.com",
                            Secret = "lcUNnsobBB5sl_1NHohHnhlh",
                            Callback = "http://alkalined.dyndns.org/account/oauth"
                        };
            //p.Authorize(Request.QueryString["code"]);
            var content = p.GetInfo(Request.QueryString["code"]);
            return new ContentResult() { Content = content, ContentType = "application/json",ContentEncoding = Encoding.UTF8};
        }

        [HttpPost]
        public ActionResult Signup(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var person = DbSession.Load<Core.Person>("people/" + register.Username);
            if (person != null)
            {
                ModelState.AddModelError("model.unique", "The username you specified already exists in the system");
                return View();
            }

            person = Core.Person.Forge(register.Email, register.Username, register.Password);
            DbSession.Store(person);
            return RedirectToAction("View", "Home");
        }
    }
}