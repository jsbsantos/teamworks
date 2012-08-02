using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LowercaseRoutesMVC4;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Mvc;


namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class App : HttpApplication
    {
        public static class Keys
        {
            public const string RavenDbSessionKey = "RAVENDB_SESSION_KEY";
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FormsAuthenticationAttribute());
        }

        public static void RegisterGlobalApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new ModelStateAttribute());

            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof (ReadVetoException),
                                new ExceptionAttribute.Rule {Status = HttpStatusCode.NotFound});
            filter.Mappings.Add(typeof (ArgumentException),
                                new ExceptionAttribute.Rule {HasBody = true, Status = HttpStatusCode.BadRequest});
            filters.Add(filter);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "",
                url: "projects/{projectid}/{controller}/{identifier}",
                defaults: new {action = "View", identifier = UrlParameter.Optional}
                );

            routes.MapRouteLowercase(
                name: "",
                url: "projects/{identifier}",
                defaults: new {controller = "Projects", action = "View", identifier = UrlParameter.Optional}
                );

            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "View", id = UrlParameter.Optional}
                );
        }

        public static void RegisterIndixes(IDocumentStore store)
        {
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(Timelog_Filter).Assembly, store);
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(ActivityWithDurationIndex).Assembly, store);
            
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
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            BundleTable.Bundles.EnableTeamworksBundle();
            Mappers.RegisterMappers();

            var store = 
                new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.Initialize();
            Global.Store = store;
            RegisterIndixes(store);
			Global.Authentication.Add("Basic", new BasicAuthenticator());
        }
    }
}