using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Teamworks.Core.Authentication;
using Teamworks.Web.Controllers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class ViewsExtensions
    {
        public static HelperResult ActiveController(this HtmlHelper _this, string controller)
        {
            string c = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            return new HelperResult(
                writer => writer.Write(controller.Equals(c, StringComparison.OrdinalIgnoreCase) ? "active" : "")
                );
        }

        public static HelperResult ActiveControllerAndAction(this HtmlHelper _this, string controller, string action)
        {
            string c = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
            string a = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");

            bool condition = controller.Equals(c, StringComparison.OrdinalIgnoreCase)
                             && action.Equals(a, StringComparison.OrdinalIgnoreCase);

            return new HelperResult(
                writer => writer.Write(condition ? "active" : "")
                );
        }

        public static HelperResult Identity(this HtmlHelper _this)
        {
            IPrincipal user = HttpContext.Current.User;
            var identity = user.Identity as PersonIdentity;
            return user.Identity.IsAuthenticated && identity != null
                       ? new HelperResult(writer => _this.RenderPartial("Partial/Authenticated", (identity).Person.MapTo<PersonViewModel>()))
                       : new HelperResult(writer => _this.RenderPartial("Partial/Anonymous"));
        }

        public static HelperResult ToJson(this HtmlHelper helper, object model)
        {
            return new HelperResult(
                writer => writer.Write(Utils.ToJson(model)));
        }


        public static HelperResult ToJson<T>(this HtmlHelper helper, T model)
        {
            return new HelperResult(
                writer => writer.Write(Utils.ToJson(model)));
        }

        public static HelperResult ToIndentedJson(this HtmlHelper helper, object model)
        {
            return new HelperResult(
                writer => writer.Write(Utils.ToIndentedJson(model)));
        }

        public static HelperResult ToIndentedJson<T>(this HtmlHelper helper, T model)
        {
            return new HelperResult(
                writer => writer.Write(Utils.ToIndentedJson(model)));
        }

        public static HelperResult Breadcrumb(this HtmlHelper helper, AppController.Breadcrumb[] breadcrumb)
        {
            if (breadcrumb == null || breadcrumb.Length == 0)
            {
                return new HelperResult(writer => { });
            }
            
            const string last = "<li><span data-bind='text: name'>{0}</span></li>";
            const string template = "<li><a href='{0}'>{1}</a><span class='divider'>/</span></li>";
            return new HelperResult(writer =>
                {
                    int size = breadcrumb.Length;
                    writer.WriteLine("<ul class='breadcrumb'>");
                    for (var i = 0; i < size; i++)
                    {
                        var item = breadcrumb[i];
                        if (i < size - 1)
                        {
                            writer.WriteLine(template, item.Url, item.Name);
                            continue;    
                        }
                        writer.WriteLine(last, item.Name);
                    }
                    writer.WriteLine("</ul>");
                });
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