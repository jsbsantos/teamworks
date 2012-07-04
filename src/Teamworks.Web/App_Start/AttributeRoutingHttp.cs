using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using Teamworks.Web.Controllers.Api;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Teamworks.Web.App_Start.AttributeRoutingHttp), "Start")]
namespace Teamworks.Web.App_Start {
    public static class AttributeRoutingHttp {
		public static void RegisterRoutes(RouteCollection routes) {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

			// ASP.NET Web API
            routes.MapHttpAttributeRoutes(c =>
                                              {
                                                  c.ScanAssembly(Assembly.GetExecutingAssembly());
                                                  c.AddRoutesFromController<ProjectsController>();
                                                  c.AddRoutesFromController<MessagesController>();
                                                  c.UseLowercaseRoutes = true;
                                              });
		}

        public static void Start() {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
