using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using Teamworks.Web;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AttributeRoutingHttp), "Start")]
namespace Teamworks.Web
{
    public static class AttributeRoutingHttp
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            routes.MapHttpAttributeRoutes(c =>
            {
                c.ScanAssembly(Assembly.GetExecutingAssembly()); c.UseLowercaseRoutes = true;
            });
        }

        public static void Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
