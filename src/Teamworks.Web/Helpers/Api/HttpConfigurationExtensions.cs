using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Teamworks.Web.Controllers.Api.Handlers;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpConfigurationExtensions
    {
        public static void RegisterModelBinders(this HttpConfiguration configuration)
        {
            /*
            var modelBinderProviderServices = configuration.ServiceResolver.GetServices(typeof (ModelBinderProvider));
            var services = new List<object>(modelBinderProviderServices) {new MailgunModelBinderProvider()};
            configuration.ServiceResolver.SetServices(typeof (ModelBinderProvider), services.ToArray());
             * */
        }

        public static void RegisterWebApiHandlers(this HttpConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new UnauthorizedHandler());
            configuration.MessageHandlers.Add(new BasicAuthenticationHandler());
            configuration.MessageHandlers.Add(new FormsAuthenticationHandler());
            configuration.MessageHandlers.Add(new RavenHandler());
        }
    }
}