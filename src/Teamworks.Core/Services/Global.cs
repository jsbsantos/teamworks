using System;
using System.Text;
using Teamworks.Core.Authentication;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static FactoryOfAuthenticators Authentication
        {
            get { return FactoryOfAuthenticators.Instance; }
        }

        public static Services.Raven Raven
        {
            get { return Services.Raven.Instance; }
        }

        public static Tuple<string, string> DecodeBasicAuthenticationHeader(string basicAuthToken)
        {
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