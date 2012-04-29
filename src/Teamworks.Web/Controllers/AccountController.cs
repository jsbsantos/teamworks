using System.Dynamic;
using System.Web.Mvc;

namespace Teamworks.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            dynamic o = new ExpandoObject();
            o.Username = username;
            o.Password = password;

            var state = AuthenticationManager.Validate("BasicWeb",
                AuthenticationManager.GetCredentials("BasicWeb", o));
            return new ContentResult() { Content = (state ? "" : "not") + "authenticated", ContentType = "text/html" };
        }
    }
}