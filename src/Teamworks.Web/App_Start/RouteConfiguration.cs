using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using LowercaseRoutesMVC4;
using Teamworks.Web.Controllers.Api;

namespace Teamworks.Web.App_Start
{
    public static class RouteConfiguration
    {
        public static void Register(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpAttributeRoutes(c =>
            {
                c.AddRoutesFromController<ProjectsController>();

                c.AutoGenerateRouteNames = true;
                c.UseLowercaseRoutes = true;
            });

            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
                "homepage",
                "",
                new { controller = "Home", action = "Index" }
                );
            
        }
    }
}