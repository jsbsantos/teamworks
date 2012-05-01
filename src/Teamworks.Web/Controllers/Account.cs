using System.Dynamic;
using System.Web.Mvc;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Controllers {
    public class AccountController : RavenController {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Login() {
            dynamic o = new ExpandoObject();
            o.Username = HttpContext.Request.Form["username"];
            o.Password = HttpContext.Request.Form["password"];

            var state = AuthenticationManager.Validate("BasicWeb",
                                                       AuthenticationManager.GetCredentials("BasicWeb", o));
            return new ContentResult() {Content = (state ? "" : "not") + "authenticated", ContentType = "text/html"};
        }
    }
}