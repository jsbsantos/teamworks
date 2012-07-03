using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Controllers.Api.Attribute;
using Teamworks.Web.Controllers.Mvc.Attributes;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Mvc;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class TeamworksApp : HttpApplication
    {
        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FormsAuthenticationAttribute());
        }

        public static void RegisterGlobalApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new ModelStateAttribute());
            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
            filters.Add(filter);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "",
                 url: "projects/{projectid}/{controller}/{identifier}",
                 defaults: new { action = "View", identifier = UrlParameter.Optional }
                 );

            routes.MapRoute(
                 name: "",
                 url: "projects/{identifier}",
                 defaults: new { controller = "Projects", action = "View", identifier = UrlParameter.Optional }
                 );

            routes.MapRoute(
                 name: "default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                 );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            try
            {
                new Uri("http://fail/first/time?only=%2bplus");
            }
            catch (Exception)
            {
            }

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterGlobalApiFilters(GlobalConfiguration.Configuration.Filters);
            RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.ConfigureJsonNet();
            GlobalConfiguration.Configuration.RegisterWebApiHandlers();
            GlobalConfiguration.Configuration.RegisterModelBinders();

            BundleTable.Bundles.EnableTeamworksBundle();

            Global.Authentication.Add("Basic", new BasicAuthenticator());
            Mappers.RegisterMappers();
        }

        
    }
}