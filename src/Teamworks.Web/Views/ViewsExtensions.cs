﻿using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Teamworks.Web.Views {
    public static class ViewsExtensions {
        public static HelperResult ActiveController(this HtmlHelper _this, string controller) {
            string name = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            return new HelperResult(
                writer => writer.Write(controller.Equals(name, StringComparison.OrdinalIgnoreCase) ? "active" : "")
                );
        }

        public static HelperResult Identity(this HtmlHelper _this) {
            IPrincipal user = HttpContext.Current.User;
            return
                new HelperResult(
                    writer =>
                    _this.RenderPartial(user.Identity.IsAuthenticated ? "Authenticated" : "Anonymous"));
        }

        public static bool IsDebugging(this HtmlHelper helper) {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}