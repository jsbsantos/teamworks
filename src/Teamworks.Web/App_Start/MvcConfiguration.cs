using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Optimization;
using Raven.Client.Exceptions;
using Teamworks.Web.Attributes.Mvc;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.App_Start
{
    public static class MvcConfiguration
    {
        public static void Register()
        {
            RegisterFilters(GlobalFilters.Filters);

            BundleTable.Bundles.EnableTeamworksBundle();
        }

        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute {View = "Errors/500"});
            filters.Add(new HandleErrorAttribute {ExceptionType = typeof (SocketException), View = "Erros/NoDb"});
            filters.Add(new HandleErrorAttribute {ExceptionType = typeof (ReadVetoException), View = "Errors/404"});

            filters.Add(new FormsAuthenticationAttribute(), -2);
        }
    }
}