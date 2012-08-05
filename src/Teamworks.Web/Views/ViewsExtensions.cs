using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Views
{
    public static class ViewsExtensions
    {
        public static HelperResult ActiveController(this HtmlHelper _this, string controller)
        {
            var c = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            return new HelperResult(
                writer => writer.Write(controller.Equals(c, StringComparison.OrdinalIgnoreCase) ? "active" : "")
                );
        }

        public static HelperResult ActiveControllerAndAction(this HtmlHelper _this, string controller, string action)
        {
            var c = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            var a = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");

            var condition = controller.Equals(c, StringComparison.OrdinalIgnoreCase)
                            && action.Equals(a, StringComparison.OrdinalIgnoreCase);

            return new HelperResult(
                writer => writer.Write(condition ? "active" : "")
                );
        }

        public static HelperResult Identity(this HtmlHelper _this)
        {
            var user = HttpContext.Current.User;
            var identity = user.Identity as PersonIdentity;
            return user.Identity.IsAuthenticated && identity != null
                       ? new HelperResult(writer => _this.RenderPartial("Authenticated", (identity).Person))
                       : new HelperResult(writer => _this.RenderPartial("Anonymous"));
        }

        public static bool IsDebugging(this HtmlHelper helper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}