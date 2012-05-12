using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Security;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string username, string password)
        {
            dynamic dyn = new ExpandoObject();
            dyn.Username = username;
            dyn.Password = password;

            if (AuthenticationManager.Validate("BasicWeb", AuthenticationManager.GetCredentials("BasicWeb", dyn)))
            {
                FormsAuthentication.SetAuthCookie(dyn.Username, false);
                return Redirect(FormsAuthentication.GetRedirectUrl(dyn.Username, false));    
            }

            var errors = TempData["ERRORS_LIST"] as List<string> ?? new List<string>();
            errors.Add("The username or password you entered is incorrect.");
            TempData["ERRORS_LIST"] = errors;
            return RedirectToAction("View");
        }

        [HttpGet]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("View", "Home");
        }

        [HttpGet]
        public ActionResult Signup() {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Register register) {
            if (!ModelState.IsValid)
                return View();

            var person = Person.Forge(register.Email, register.Username, register.Password);
            DbSession.Store(person);
            return RedirectToAction("View", "Home");
        }

    }

    public class Register {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}