using System.Web;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Extensions.Mvc
{
    public static class HttpContextExtensions
    {
        public static Person GetCurrentPerson(this IDocumentSession context)
        {
            var person = HttpContext.Current.User.Identity as PersonIdentity;
            return person == null ? null : person.Person;
        }

        public static string GetCurrentPersonId(this IDocumentSession context)
        {
            Person person = GetCurrentPerson(context);
            return person == null ? null : person.Id;
        }

        public static IDocumentSession GetCurrentRavenSession(this HttpContextBase context)
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