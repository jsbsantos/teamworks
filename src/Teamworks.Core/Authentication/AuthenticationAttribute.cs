using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Teamworks.Core.Authentication {
    //Web Controller Authentication Attribute
    public class AuthenticationAttribute : AuthorizeAttribute {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            HttpContextBase context = filterContext.HttpContext;
            if (context.Request.Cookies["teamworks_sessionid"] == null) {
                context.Response.Redirect("Home/Login", true);
                return;
            }

            HttpCookie sessionId = context.Request.Cookies["teamworks_sessionid"];
            Session session = Session.Get(sessionId.Value);
            if (session == null) {
                context.Response.Redirect("Home/Login", true);
                return;
            }

            //todo change principal
            context.User = new GenericPrincipal(new GenericIdentity(session.Person.Username), null);
        }
    }
}