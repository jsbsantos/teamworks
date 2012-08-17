using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof (Teamworks.Web.App_Start.AttributeRouting), "Start")]

namespace Teamworks.Web.App_Start
{
    public static class AttributeRouting
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // this method does nothing but it's here so restore pacakage from nuget don't
            // change it, see RouteConfiguration.Register
        }

        public static void Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}