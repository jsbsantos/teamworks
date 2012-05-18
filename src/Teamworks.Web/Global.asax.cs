using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
using Microsoft.Web.Optimization;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Handlers;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "View", id = UrlParameter.Optional}
                );
        }

        public static void RegisterWebApiHandlers()
        {
            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            configuration.MessageHandlers.Add(new RavenHandler());
            configuration.MessageHandlers.Add(new AuthHandler());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterWebApiHandlers();
            AutoMapperSetup.RegisterMappers();

            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            configuration.Formatters.Remove(configuration.Formatters.JsonFormatter);
            configuration.Formatters.Add(new JsonNetFormatter());


            Global.Authentication.Add("Basic", new BasicAuthenticator());
            BundleTable.Bundles.EnableTeamworksBundle();
        }
    }
}