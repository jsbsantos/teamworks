using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using AutoMapper;
using LowercaseRoutesMVC;
using Microsoft.Web.Optimization;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Handlers;
using Teamworks.Web.Models;

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

            routes.MapHttpAttributeRoutes(c =>
            {
                c.ScanAssembly(Assembly.GetExecutingAssembly()); c.UseLowercaseRoutes = true;
            });

            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                );

        }

        public static void RegisterMappers()
        {
            Mapper.CreateMap<Project, Core.Projects.Project>();
            Mapper.CreateMap<Core.Projects.Project, Project>().ForMember(src => src.Tasks,
                                                                         opt =>
                                                                         opt.MapFrom(
                                                                             src => Mapper.Map<IList<Core.Projects.Task>, IList<Task>>(Global.Raven.CurrentSession.Load<Core.Projects.Task>(
                                                                                 src.TaskIds))
                                                                             ))
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
            Mapper.CreateMap<Task, Core.Projects.Task>();
            Mapper.CreateMap<Core.Projects.Task, Task>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
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
            RegisterMappers();

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