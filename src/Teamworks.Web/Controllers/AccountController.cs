using System.Dynamic;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Controllers
{
    public class AccountController : RavenController
    {
        /*[HttpGet]
        public ActionResult Index()
        {
            return View("View");
        }

        public ActionResult Login()
        {
            dynamic o = new ExpandoObject();
            o.Username = HttpContext.Request.Form["username"];
            o.Password = HttpContext.Request.Form["password"];

            dynamic state = AuthenticationManager.Validate("BasicWeb",
                                                           AuthenticationManager.GetCredentials("BasicWeb", o));
            return new ContentResult { Content = (state ? "" : "not") + "authenticated", ContentType = "text/html" };
        }
        */

        public ActionResult Index()
        {
            if (HttpContext.Request.HttpMethod == HttpMethod.Get.ToString())
                return View("View");

            dynamic o = new ExpandoObject();
            o.Username = HttpContext.Request.Form["username"];
            o.Password = HttpContext.Request.Form["password"];

            dynamic state = AuthenticationManager.Validate("BasicWeb",
                                                           AuthenticationManager.GetCredentials("BasicWeb", o));
            
            FormsAuthentication.SetAuthCookie(o.Username+"8",false);
            //HttpContext.User = new GenericPrincipal(new GenericIdentity(o.Username), null);

            FormsAuthentication.RedirectFromLoginPage(o.Username, false);


            return new ContentResult { Content = (state ? "" : "not") + "authenticated", ContentType = "text/html" };
        }

    }
}