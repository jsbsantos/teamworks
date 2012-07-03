using System.Net.Http;
using System.Threading;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpRequestMessageExtensions
    {
        private const string QueryStringKey = "QUERY_STRING_KEY";

        public static Person GetCurrentPerson(this HttpRequestMessage request)
        {
            var principal = Thread.CurrentPrincipal;
            if (principal == null)
            {
                return null;
            }

            var person = principal.Identity as PersonIdentity; 
            return person == null ? null : person.Person;
        }

        public static string GetCurrentPersonId(this HttpRequestMessage request)
        {
            var person = GetCurrentPerson(request);
            return person == null ? "" : person.Id;
        }
    }
}