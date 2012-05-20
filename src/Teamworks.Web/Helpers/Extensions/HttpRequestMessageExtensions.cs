using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        private const string QueryStringKey = "QUERY_STRING_KEY";

        public static Person GetCurrentPerson(this HttpRequestMessage request)
        {
            var person = request.GetUserPrincipal().Identity as PersonIdentity;
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