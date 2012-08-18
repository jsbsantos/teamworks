using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using AttributeRouting.Web.Mvc;
using LowercaseRoutesMVC4;
using Teamworks.Web.Controllers;

namespace Teamworks.Web.App_Start
{
    public static class RouteConfiguration
    {
        public static void Register(RouteCollection routes)
        {
            routes.Clear();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapHttpAttributeRoutes(c =>
                                              {
                                                  c.ScanAssembly(Assembly.GetExecutingAssembly());
                                                  c.AutoGenerateRouteNames = true;
                                                  c.UseLowercaseRoutes = true;
                                                  c.AppendTrailingSlash = true;
                                                  c.RouteNameBuilder = specification =>
                                                                       "api_" +
                                                                       specification.ControllerName.
                                                                           ToLowerInvariant() +
                                                                       "_" +
                                                                       specification.ActionName.
                                                                           ToLowerInvariant();
                                              });
            routes.MapAttributeRoutes(c =>
                                          {
                                              c.ScanAssembly(Assembly.GetExecutingAssembly());
                                              c.AutoGenerateRouteNames = true;
                                              c.UseLowercaseRoutes = true;
                                              c.AppendTrailingSlash = true;
                                              c.RouteNameBuilder = specification =>
                                                                   specification.ControllerName.
                                                                       ToLowerInvariant() +
                                                                   "_" +
                                                                   specification.ActionName.ToLowerInvariant();
                                          });
            
            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRouteLowercase(
                "homepage",
                "",
                new {controller = "Home", action = "Index"}
                );
        }
    }
}