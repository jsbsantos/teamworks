using System.Web;
using Teamworks.Core;
using Teamworks.Core.Authentication;

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
    }
}