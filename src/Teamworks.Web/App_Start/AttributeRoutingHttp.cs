using System.Reflection;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using Teamworks.Web.App_Start;
using Teamworks.Web.Controllers.Api;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof (AttributeRoutingHttp), "Start")]

namespace Teamworks.Web.App_Start
{
    public static class AttributeRoutingHttp
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            // ASP.NET Web API
            routes.MapHttpAttributeRoutes(c =>
                                              {
                                                  //c.ScanAssembly(Assembly.GetExecutingAssembly());
                                                  //c.AddRoutesFromController<HomeController>();
                                                  c.AddRoutesFromController<ProjectsController>();
                                                  //c.AddRoutesFromController<ActivitiesController>();
                                                  
                                                  //c.AddRoutesFromController<MessagesController>();
                                                  c.AutoGenerateRouteNames = true;
                                                  c.UseLowercaseRoutes = true;
                                              });
        }

        public static void Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}