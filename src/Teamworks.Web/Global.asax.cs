using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Teamworks.Web.App_Start;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class Application : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            MvcConfiguration.Register();
            ApiConfiguration.Register(GlobalConfiguration.Configuration);

            RouteConfiguration.Register(RouteTable.Routes);
            ApplicationConfiguration.Register();
        }

        #region Nested type: Keys

        public static class Keys
        {
            public const string RavenDbSessionKey = "RAVENDB_SESSION_KEY";
        }

        #endregion
    }
}