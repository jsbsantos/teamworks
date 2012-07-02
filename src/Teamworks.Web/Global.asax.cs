using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
using Microsoft.Web.Optimization;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api.Attribute;
using Teamworks.Web.Controllers.Mvc.Attributes;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Mvc;
using GlobalFilterCollection = System.Web.Http.Filters.GlobalFilterCollection;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class TeamworksApp : HttpApplication
    {
        public void RegisterGlobalFilters(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FormsAuthenticationAttribute());
        }

        public static void RegisterGlobalApiFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ModelStateAttribute());
            
            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
            filters.Add(filter);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                 name: "",
                 url: "projects/{projectid}/{controller}/{identifier}",
                 defaults: new { action = "View", identifier = UrlParameter.Optional }
                 );

            routes.MapRouteLowercase(
                 name: "",
                 url: "projects/{identifier}",
                 defaults: new { controller = "Projects", action = "View", identifier = UrlParameter.Optional }
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
            RegisterGlobalApiFilters(GlobalConfiguration.Configuration.Filters);
            RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.RegisterFormatters();
            GlobalConfiguration.Configuration.RegisterWebApiHandlers();

            BundleTable.Bundles.EnableTeamworksBundle();

            Global.Authentication.Add("Basic", new BasicAuthenticator());
            Mappers.RegisterMappers();
        }

        
    }
}