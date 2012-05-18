using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
using Microsoft.Web.Optimization;
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
                defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                );


            routes.MapRouteLowercase(
                name: "project_task",
                url: "projects/{projectid}/tasks/{id}",
                defaults: new { controller = "Tasks", action = "View", id = UrlParameter.Optional, projectid = UrlParameter.Optional }
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


            AuthenticationManager.Add("Basic", new BasicAuthenticationHandler());
            AuthenticationManager.Add("BasicWeb", new BasicWebAuthenticationHandler());
            AuthenticationManager.DefaultAuthenticationScheme = "BasicWeb";

            BundleTable.Bundles.EnableTeamworksBundle();
        }
    }
}