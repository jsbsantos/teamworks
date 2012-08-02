using System.Web;
using Raven.Client;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class HttpContextBaseExtensions
    {
        public static IDocumentSession GetOrOpenCurrentSession(this HttpContextBase context)
        {
            var session = context.Items[App.Keys.RavenDbSessionKey] as IDocumentSession;
            if (session == null)
            {
                session = Global.Store.OpenSession();
                context.Items[App.Keys.RavenDbSessionKey] = session;
            }
            return session;
        }
         
    }
}