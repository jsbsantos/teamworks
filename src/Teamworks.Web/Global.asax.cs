using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
using Microsoft.Web.Optimization;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Extensions;

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
                name: "project_task_timelog",
                url: "projects/{projectid}/tasks/{taskid}/timelog/{id}/{action}",
                defaults:
                    new
                    {
                        controller = "TimeEntry",
                        action = "View",
                        id = UrlParameter.Optional
                    }
                );

            routes.MapRouteLowercase(
                name: "project_task",
                url: "projects/{projectid}/tasks/{id}/{action}",
                defaults:
                    new
                    {
                        controller = "Tasks",
                        action = "View",
                        id = UrlParameter.Optional
                    }
                );

            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.RegisterFormatters();
            GlobalConfiguration.Configuration.RegisterWebApiHandlers();

            BundleTable.Bundles.EnableTeamworksBundle();

            Global.Authentication.Add("Basic", new BasicAuthenticator());
            Mappers.RegisterMappers();
        }
    }
}