using System.Web;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Extensions
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
            var person = GetCurrentPerson(context);
            return person == null ? "" : person.Id;
        }
    }
}