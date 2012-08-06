using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LowercaseRoutesMVC4;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Exceptions;
using Raven.Client.Indexes;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Attributes.Api.Ordered;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Handlers;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Mvc;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class Application : HttpApplication
    {
        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FormsAuthenticationAttribute(), 1);
        }

        public static void RegisterGlobalApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new RavenSessionAttribute());
            filters.Add(new BasicAuthenticationAttribute());
            filters.Add(new FormsAuthenticationFilter());

            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof (ReadVetoException),
                                new ExceptionAttribute.Rule {Status = HttpStatusCode.NotFound});
            filter.Mappings.Add(typeof (ArgumentException),
                                new ExceptionAttribute.Rule {HasBody = true, Status = HttpStatusCode.BadRequest});
            filters.Add(filter);

            filters.Add(new ModelStateAttribute());
        }

        public static void RegisterGlobalWebApiHandlers(Collection<DelegatingHandler> messageHandlers)
        {
            messageHandlers.Add(new UnauthorizedHandler());
            messageHandlers.Add(new RavenSessionHandler());
        }

        public static void AppGlobalConfiguration(HttpConfiguration configuration)
        {
            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            configuration.Services.Add(typeof (ModelBinderProvider), new MailgunModelBinderProvider());

            configuration.Services.Add(typeof (System.Web.Http.Filters.IFilterProvider), new OrderedFilterProvider());
            var providers = configuration.Services.GetFilterProviders();
            var defaultprovider = providers.First(i => i is ActionDescriptorFilterProvider);
            configuration.Services.Remove(typeof (System.Web.Http.Filters.IFilterProvider), defaultprovider);

            configuration.Services.Add(typeof (ModelBinderProvider), new MailgunModelBinderProvider());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
               name: "discussions",
               url: "projects/{project}/discussions/{action}/{id}",
               defaults: new { controller = "Discussions", action = "View", id = UrlParameter.Optional });

            routes.MapRouteLowercase(
                name: "activities",
                url: "projects/{project}/activities/{action}/{id}",
                defaults: new {controller = "Activities", action = "View", id = UrlParameter.Optional});

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

            InitializeExecutor();
        }

        public static void InitializeDocumentStore()
        {
            if (Global.Database != null) return; // prevent misuse

            Global.Database =
                new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.RegisterListener(new DocumentStoreListener()).Initialize();

            TryCreatingIndexesOrRedirectToErrorPage(Global.Database);
        }

        public static void TryCreatingIndexesOrRedirectToErrorPage(IDocumentStore store)
        {
            IndexCreation.CreateIndexes(typeof (Activities_ByProject).Assembly, store);
        }

        public static void InitializeExecutor()
        {
            Global.Executor = Executor.Instance;
            Global.Executor.Timeout = 15000;
            Global.Executor.Initialize();
        }

        #region Nested type: Keys

        public static class Keys
        {
            public const string RavenDbSessionKey = "RAVENDB_SESSION_KEY";
        }

        #endregion
    }
}