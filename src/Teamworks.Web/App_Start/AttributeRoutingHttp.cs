using System.Web.Routing;
using Teamworks.Web.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AttributeRoutingHttp), "Start")]

namespace Teamworks.Web.App_Start
{
    public static class AttributeRoutingHttp
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // because of this issue https://github.com/mccalltd/AttributeRouting/issues/112
            // this method does nothing but it's here so restore pacakage from nuget don't
            // mess with the workaround
        }

        public static void Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        
    }
}