using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Security;
using Teamworks.Core;
using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Web
{
    [AllowAnonymous]
    public class AccountController : RavenController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            dynamic dyn = new ExpandoObject();
            dyn.Username = model.Username;
            dyn.Password = model.Password;

            Person person;
            var auth = Global.Authentication["Basic"];
            if (auth.IsValid(dyn, out person))
            {
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

    public class Login
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool Persist { get; set; }
    }

    public class Register
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}