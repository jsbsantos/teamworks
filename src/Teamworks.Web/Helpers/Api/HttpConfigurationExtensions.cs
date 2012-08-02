using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Teamworks.Web.Handlers;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpConfigurationExtensions
    {
        public static void ConfigureJsonNet(this HttpConfiguration configuration)
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new LowercaseContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public static void RegisterModelBinders(this HttpConfiguration configuration)
        {
            configuration.Services.Add(typeof (ModelBinderProvider), new MailgunModelBinderProvider());
        }

        public static void RegisterWebApiHandlers(this HttpConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new RavenDbSessionHandler());
            configuration.MessageHandlers.Add(new BasicAuthenticationHandler());
            configuration.MessageHandlers.Add(new FormsAuthenticationHandler());
            configuration.MessageHandlers.Add(new UnauthorizedHandler());
       }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                var list = new List<char>();
                for (var i = 0; i < propertyName.Length; i++)
                {
                    if (i > 0 && char.IsUpper(propertyName[i]))
                    {
                        list.Add('_');
                    }
                    list.Add(propertyName[i]);
                }
                return new string(list.ToArray()).ToLowerInvariant();
            }
        }
    }
}