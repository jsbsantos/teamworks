using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Hosting;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpRequestMessageExtensions
    {
        private const string QueryStringKey = "QUERY_STRING_KEY";

        public static Person GetCurrentPerson(this HttpRequestMessage request)
        {
            var principal = request.Properties[HttpPropertyKeys.UserPrincipalKey] as IPrincipal;
            if (principal == null)
            {
                return null;
            }

            var person = principal.Identity as PersonIdentity; 
            return person == null ? null : person.Person;
        }

        public static string GetUserPrincipalId(this HttpRequestMessage request)
        {
            Person person = GetCurrentPerson(request);
            return person == null ? "" : person.Id;
        }

        public static string QueryString(this HttpRequestMessage request, string name)
        {
            var dict = HttpContext.Current.Items[QueryStringKey] as IDictionary<string, string>;

            string value;
            if (dict != null)
            {
                return dict.TryGetValue(name, out value) ? value : null;
            }

            dict = new Dictionary<string, string>();
            foreach (string param in request.RequestUri.Query.TrimStart(new[] {'?'})
                .Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] split = param.Split('=');

                string k = split[0];
                string v = split[1];

                dict.Add(k, v);
            }
            HttpContext.Current.Items[QueryStringKey] = dict;
            return dict.TryGetValue(name, out value) ? value : null;
        }
    }
}