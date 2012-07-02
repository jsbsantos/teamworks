using System.Dynamic;
using System.Web.Mvc;
using System.Web.Security;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Models;
using Teamworks.Web.Models.Mvc;
using Person = Teamworks.Core.Person;

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

            Person person;
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

        [HttpPost]
        public ActionResult Signup(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Person person = Person.Forge(register.Email, register.Username, register.Password);
            DbSession.Store(person);
            return RedirectToAction("View", "Home");
        }
    }
}