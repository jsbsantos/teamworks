using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Exceptions;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Attributes.Api.Ordered;
using Teamworks.Web.Controllers.Api;
using Teamworks.Web.Handlers;
using Teamworks.Web.Helpers;

namespace Teamworks.Web.App_Start
{
    public static class ApiConfiguration
    {
        public static void Register(HttpConfiguration configuration)
        {
            Registerhandlers(configuration.MessageHandlers);
            RegisterFilters(configuration.Filters);
            RegisterRoutes(RouteTable.Routes);

            FormattersConfiguration(configuration);
            ServicesConfiguration(configuration);
        }

        private static void Registerhandlers(Collection<DelegatingHandler> handlers)
        {
            handlers.Add(new RavenSession());
            handlers.Add(new BasicAuthentication());
        }

        private static void RegisterFilters(HttpFilterCollection filters)
        {
            filters.Add(new RavenSessionAttribute());
            filters.Add(new FormsAuthenticationAttribute());
            filters.Add(new ModelStateValidationAttribute());

            var filter = new ExceptionAttribute();
            filter.Mappings.Add(typeof(ReadVetoException),
                                new ExceptionAttribute.Rule { Status = HttpStatusCode.NotFound });
            filter.Mappings.Add(typeof(ArgumentException),
                                new ExceptionAttribute.Rule { HasBody = true, Status = HttpStatusCode.BadRequest });
            filters.Add(filter);
        }

        public static void FormattersConfiguration(HttpConfiguration configuration)
        {
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);

            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        
        public static void ServicesConfiguration(HttpConfiguration configuration)
        {
            configuration.Services.Add(typeof(ModelBinderProvider), new MailgunModelBinderProvider());

            configuration.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            var providers = configuration.Services.GetFilterProviders();
            var defaultprovider = providers.First(i => i is ActionDescriptorFilterProvider);
            configuration.Services.Remove(typeof(IFilterProvider), defaultprovider);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpAttributeRoutes(c =>
            {
                c.AddRoutesFromController<ProjectsController>();

                c.AutoGenerateRouteNames = true;
                c.UseLowercaseRoutes = true;
            });
        }
    }
}