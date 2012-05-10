using System;
using System.Text;
using Raven.Client;
using Raven.Client.Document;

namespace Teamworks.Core.Extensions {
    public static class Global {
        private static IDocumentStore _store;

        public static string RavenKey
        {
            get { return "RAVEN_CURRENT_SESSION_KEY"; }
        }

        public static Tuple<string, string> DecodeBasicAuthenticationHeader(string basicAuthToken) {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string userPass = encoding.GetString(Convert.FromBase64String(basicAuthToken));
            int separator = userPass.IndexOf(':');

            var credential = new Tuple<string, string>(
                userPass.Substring(0, separator),
                userPass.Substring(separator + 1));

            return credential;
        }
    }
}