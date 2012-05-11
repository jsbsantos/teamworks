using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Teamworks.Core.Extensions;

namespace Teamworks.Core.Authentication {
    /// <summary>
    /// Web controllers Authentication Attribute
    /// </summary>
    public class AuthenticationAttribute : AuthorizeAttribute {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            HttpContextBase context = filterContext.HttpContext;
            if (context.Request.Cookies["teamworks_sessionid"] == null) {
                context.Response.Redirect("Home/Login", true);
                return;
            }

            var sessionId = context.Request.Cookies["teamworks_sessionid"];
            var session = Global.Raven.CurrentSession.Load<Session>(sessionId.Value);
            
            if (session == null) {
                context.Response.Redirect("Home/Login", true);
                return;
            }

            //todo change principal
            context.User = new GenericPrincipal(new GenericIdentity(session.Person.Username), null);
        }
    }
}