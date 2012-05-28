using System.Web.Http;
using Teamworks.Web.Controllers.Api.Handlers;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class HttpConfigurationExtensions
    {
        public static void RegisterFormatters(this HttpConfiguration configuration)
        {
            configuration.Formatters.Remove(configuration.Formatters.JsonFormatter);
            configuration.Formatters.Add(new JsonNetFormatter());
        }

        public static void RegisterWebApiHandlers(this HttpConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new RavenHandler());
            configuration.MessageHandlers.Add(new BasicAuthenticationHandler());
            configuration.MessageHandlers.Add(new UnauthorizedHandler());
        }
    }
}