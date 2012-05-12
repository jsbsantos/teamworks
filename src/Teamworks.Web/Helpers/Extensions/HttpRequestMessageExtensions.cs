using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Helpers.Extensions {
    public static class HttpRequestMessageExtensions {
        public static string GetUserPrincipalId(this HttpRequestMessage request) {
            var identity = request.GetUserPrincipal().Identity as PersonIdentity;
            return identity == null ? string.Empty : identity.Person.Id;
        }

        private const string QueryStringKey = "QUERY_STRING_KEY";

        public static string QueryString(this HttpRequestMessage request, string name) {
            var dict = HttpContext.Current.Items[QueryStringKey] as IDictionary<string, string>;

            string value;
            if (dict != null) {
                return dict.TryGetValue(name, out value) ? value : null;
            }

            dict = new Dictionary<string, string>();
            foreach (var param in request.RequestUri.Query.TrimStart(new[] {'?'})
                .Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries)) {
                var split = param.Split('=');

                var k = split[0];
                var v = split[1];

                dict.Add(k, v);
            }
            HttpContext.Current.Items[QueryStringKey] = dict;
            return dict.TryGetValue(name, out value) ? value : null;
        }
    }
}