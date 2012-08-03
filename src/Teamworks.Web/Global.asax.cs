using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LowercaseRoutesMVC4;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Raven.Client.Indexes;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Handlers;
using Teamworks.Web.Helpers;
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
            filters.Add(new FormsAuthenticationAttribute(), 1);
        }

        public static void RegisterGlobalApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new ModelStateAttribute());
            filters.Add(new RavenSessionAttribute());
            filters.Add(new FormsAuthenticationFilter());

            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof (ReadVetoException),
                                new ExceptionAttribute.Rule {Status = HttpStatusCode.NotFound});
            filter.Mappings.Add(typeof (ArgumentException),
                                new ExceptionAttribute.Rule {HasBody = true, Status = HttpStatusCode.BadRequest});
            filters.Add(filter);
        }

        public  static void RegisterGlobalWebApiHandlers(Collection<DelegatingHandler> messageHandlers)
        {
            messageHandlers.Add(new RavenSessionHandler());
            messageHandlers.Add(new BasicAuthenticationHandler());
            messageHandlers.Add(new UnauthorizedHandler());
        }

        public static void AppGlobalConfiguration(HttpConfiguration configuration)
        {
            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new LowercaseContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            configuration.Services.Add(typeof(ModelBinderProvider), new MailgunModelBinderProvider());
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
            RegisterGlobalWebApiHandlers(GlobalConfiguration.Configuration.MessageHandlers);

            InitializeDocumentStore();
            
            RegisterRoutes(RouteTable.Routes);
            AppGlobalConfiguration(GlobalConfiguration.Configuration);

            AutoMapperConfiguration.Configure();
            BundleTable.Bundles.EnableTeamworksBundle();
        }

        public static void InitializeDocumentStore()
        {
            if (Global.Store != null) return; // prevent misuse

            Global.Store =
                new DocumentStore
                {
                    ConnectionStringName = "RavenDB"
                }.Initialize();

            TryCreatingIndexesOrRedirectToErrorPage(Global.Store);
        }

        public static void TryCreatingIndexesOrRedirectToErrorPage(IDocumentStore store)
        {
            IndexCreation.CreateIndexes(typeof(Activities_ByProject).Assembly, store);
        }
    }
}