using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Helpers.Mvc
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
                       ? new HelperResult(writer => _this.RenderPartial("Authenticated", (identity).Person))
                       : new HelperResult(writer => _this.RenderPartial("Anonymous"));
        }

        public static HelperResult ToJson(this HtmlHelper helper, object model)
        {
            string json = JsonConvert.SerializeObject(model,
                                                      new JsonSerializerSettings
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver(),
                                                              NullValueHandling = NullValueHandling.Ignore
                                                          });
            return new HelperResult(
                writer => writer.Write(json));
        }


        public static HelperResult ToJson<T>(this HtmlHelper helper, T model)
        {
            string json = JsonConvert.SerializeObject(model,
                                                      new JsonSerializerSettings
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver(),
                                                              NullValueHandling = NullValueHandling.Ignore
                                                          });
            return new HelperResult(
                writer => writer.Write(json));
        }

        public static HelperResult ToIndentedJson(this HtmlHelper helper, object model)
        {
            string json = JsonConvert.SerializeObject(model,
                                                      Formatting.Indented,
                                                      new JsonSerializerSettings
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver(),
                                                              NullValueHandling = NullValueHandling.Ignore
                                                          });
            return new HelperResult(
                writer => writer.Write(json));
        }

        public static HelperResult ToIndentedJson<T>(this HtmlHelper helper, T model)
        {
            string json = JsonConvert.SerializeObject(model,
                                                      Formatting.Indented,
                                                      new JsonSerializerSettings
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver(),
                                                              NullValueHandling = NullValueHandling.Ignore
                                                          });
            return new HelperResult(
                writer => writer.Write(json));
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