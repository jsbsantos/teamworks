using System.Web;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class HttpContextExtensions
    {
        public static Person GetCurrentPerson(this HttpContextBase context)
        {
            var person = context.User.Identity as PersonIdentity;
            return person == null ? null : person.Person;
        }

        public static string GetUserPrincipalId(this HttpContextBase context)
        {
            Person person = GetCurrentPerson(context);
            return person == null ? "" : person.Id;
        }

        public static IDocumentSession RavenSession(this HttpContextBase context)
        {
            var session = context.Items[Application.Keys.RavenDbSessionKey] as IDocumentSession;
            if (session == null)
            {
                session = Global.Database.OpenSession();
                context.Items[Application.Keys.RavenDbSessionKey] = session;
            }
            return session;
        }
    }
}