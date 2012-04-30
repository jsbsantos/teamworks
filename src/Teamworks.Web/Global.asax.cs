
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using AutoMapper;
using LowercaseRoutesMVC4;
using Microsoft.Web.Optimization;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers;

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

            routes.MapHttpAttributeRoutes(config =>
            {
                config.ScanAssembly(Assembly.GetExecutingAssembly());
                config.UseLowercaseRoutes = true;
            });
            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                );
        }

        public static void RegisterMappers()
        {
            Mapper.CreateMap<Models.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => int.Parse(src.Id) > 0 ? src.Id : null));
            Mapper.CreateMap<Project, Models.Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterMappers();

            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            configuration.Formatters.Remove(configuration.Formatters.JsonFormatter);
            configuration.Formatters.Add(new JsonNetFormatter());
            configuration.MessageHandlers.Add(new RavenMessageHandler());


            BundleTable.Bundles.EnableTeamworksBundle();
        }
    }

}