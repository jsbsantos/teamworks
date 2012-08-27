using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Helpers.Extensions.Api
{
    public static class HttpRequestMessageExtensions
    {
        public static Person GetCurrentPerson(this HttpRequestMessage request)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal == null)
            {
                return null;
            }

            var person = principal.Identity as PersonIdentity;
            return person == null ? null : person.Person;
        }

        public static string GetCurrentPersonId(this HttpRequestMessage request)
        {
            Person person = GetCurrentPerson(request);
            return person == null ? null : person.Id;
        }

        //public static void ThrowNotFound(this HttpRequestMessage request)
        //{
        //    throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
        //}

        //public static void NotFound(this HttpRequestMessage request, object obj)
        //{
        //    if (obj == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //}
    }
}