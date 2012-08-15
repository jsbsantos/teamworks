using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Sockets;
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
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Attributes.Api.Ordered;
using Teamworks.Web.Handlers;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class Application : HttpApplication
    {
        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Attributes.Mvc.FormsAuthenticationAttribute(), 1);
        }

        public static void RegisterGlobalApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new RavenSessionAttribute());
            filters.Add(new FormsAuthenticationAttribute());

            var filter = new MappingExceptionFilterAttribute();
            filter.Mappings.Add(typeof (ReadVetoException),
                                new MappingExceptionFilterAttribute.Rule {Status = HttpStatusCode.NotFound});
            filter.Mappings.Add(typeof (ArgumentException),
                                new MappingExceptionFilterAttribute.Rule {HasBody = true, Status = HttpStatusCode.BadRequest});
            filters.Add(filter);

            filters.Add(new ModelStateAttribute());
        }

        public static void RegisterGlobalWebApiHandlers(Collection<DelegatingHandler> messageHandlers)
        {
            messageHandlers.Add(new BasicAuthentication());
            messageHandlers.Add(new RavenSession());
        }

        public static void AppGlobalConfiguration(HttpConfiguration configuration)
        {

            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);

            JsonMediaTypeFormatter json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            configuration.Services.Add(typeof (ModelBinderProvider), new MailgunModelBinderProvider());

            configuration.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new OrderedFilterProvider());
            IEnumerable<System.Web.Http.Filters.IFilterProvider> providers = configuration.Services.GetFilterProviders();
            System.Web.Http.Filters.IFilterProvider defaultprovider = providers.First(i => i is ActionDescriptorFilterProvider);
            configuration.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultprovider);

            configuration.Services.Add(typeof (ModelBinderProvider), new MailgunModelBinderProvider());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "discussions",
                url: "projects/{projectId}/discussions/{action}/{discussionId}",
                defaults: new {controller = "Discussions", action = "View", id = UrlParameter.Optional});

            routes.MapRouteLowercase(
                name: "activities",
                url: "projects/{projectId}/activities/{action}/{activityId}",
                defaults: new {controller = "Activities", action = "View", id = UrlParameter.Optional});

            routes.MapRouteLowercase(
                name: "default_profiles",
                url: "profiles/{id}",
                defaults: new {controller = "Profiles", action = "View", id = UrlParameter.Optional}
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

            InitializeExecutor();
        }

        public static void InitializeDocumentStore()
        {
            if (Global.Database != null) return; // prevent misuse

            Global.Database =
                new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.Initialize();

            TryCreatingIndexesOrRedirectToErrorPage(Global.Database);
        }

        public static void TryCreatingIndexesOrRedirectToErrorPage(IDocumentStore store)
        {
            try
            {
                IndexCreation.CreateIndexes(typeof (Activities_ByProject).Assembly, store);
            }
            catch (Exception e)
            {
                var socketException = e.InnerException as SocketException;
                if (socketException == null)
                    throw;

                switch (socketException.SocketErrorCode)
                {
                    case SocketError.AddressNotAvailable:
                    case SocketError.NetworkDown:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                    case SocketError.TimedOut:
                    case SocketError.ConnectionRefused:
                    case SocketError.HostDown:
                    case SocketError.HostUnreachable:
                    case SocketError.HostNotFound:
                        // todo redirect to pretty page    
                        break;
                    default:
                        throw;
                }
            }
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