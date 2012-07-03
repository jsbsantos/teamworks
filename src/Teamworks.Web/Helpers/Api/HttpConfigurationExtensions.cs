using System.Web.Http;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json.Serialization;
using Teamworks.Web.Controllers.Api.Handlers;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpConfigurationExtensions
    {
        public static void ConfigureJsonNet(this HttpConfiguration configuration)
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new LowercaseContractResolver();
        }

        public static void RegisterModelBinders(this HttpConfiguration configuration)
        {
            configuration.Services.Add(typeof(ModelBinderProvider), new MailgunModelBinderProvider());
        }

        public static void RegisterWebApiHandlers(this HttpConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new BasicAuthenticationHandler());
            configuration.MessageHandlers.Add(new UnauthorizedHandler());
            configuration.MessageHandlers.Add(new RavenHandler());
        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }
    }
}