using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Http.WebHost;
using LowercaseRoutesMVC4;
using Microsoft.Web.Optimization;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Authentication;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;
using Teamworks.Web.Helpers;
using Teamworks.Web.Models;

namespace Teamworks.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
            this.PreRequestHandlerExecute += (sender, args) =>
            {
                if (Local.Data[Global.RavenSessionkey] == null)
                {
                    var store = new DocumentStore() { ConnectionStringName = "RavenDB" }.Initialize();
                    Local.Data[Global.RavenSessionkey] = store.OpenSession();
                }
            };

            EndRequest += (sender, args) =>
            {
                IDocumentSession session = Local.Data[Global.RavenSessionkey] as IDocumentSession;

                if (session != null && Server.GetLastError() != null)
                    session.SaveChanges();
            };
        }
        public static IDocumentStore DocumentStore { get; private set; }
        
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpAttributeRoutes(config =>
            {
                config.ScanAssembly(Assembly.GetExecutingAssembly());
                config.UseLowercaseRoutes = true;
            });
            routes.MapRouteLowercase(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
                );
        }

        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            HttpConfiguration configuration = GlobalConfiguration.Configuration;
            configuration.Formatters.Remove(configuration.Formatters.JsonFormatter);
            configuration.Formatters.Add(new JsonNetFormatter());

            //API Authentication Config
            configuration.MessageHandlers.Add(new AuthenticationHandler());
            AuthenticationManager.Add("Basic", new BasicAuthenticationHandler());
            AuthenticationManager.Add("BasicWeb", new BasicWebAuthenticationHandler());
            AuthenticationManager.DefaultAuthenticationScheme = "Basic";

            BundleTable.Bundles.EnableTeamworksBundle();
            init();
        }

        //todo remove
        private void init()
        {
            DocumentStore = new DocumentStore() { ConnectionStringName = "RavenDB" }.Initialize();
            using (var session = DocumentStore.OpenSession())
            {
                Local.Data[Global.RavenSessionkey] = session;

                var person = Person.Get("person/1");
                if (person == null)
                    Person.Add(new Person("email", Person.EncodePassword("password"), "username"));
                session.SaveChanges();
            }
        }


    }
}