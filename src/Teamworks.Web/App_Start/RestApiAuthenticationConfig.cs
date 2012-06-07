using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Teamworks.Web.App_Start;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Api;

[assembly: PreApplicationStartMethod(typeof (RestApiAuthenticationConfig), "Start")]

namespace Teamworks.Web.App_Start
{
    public static class RestApiAuthenticationConfig
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (RestApiSupressAuthenticationRedirectModule));
        }
    }
}