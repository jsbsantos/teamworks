using System;
using System.Text;

namespace Teamworks.Core {
    public static class Global {
        public static Raven Raven {
            get { return Raven.Instance; }
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