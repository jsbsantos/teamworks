using System;
using System.Net;

namespace Teamworks.Core.Authentication
{
    public class BasicAuthenticationHandler : WebAuthentication
    {
        public override NetworkCredential GetCredentials(dynamic token)
        {
            var tk = token as string;
            if (String.IsNullOrWhiteSpace(tk))
            {
                throw new ArgumentNullException("token");
            }
            Tuple<string, string> cred = Global.DecodeBasicAuthenticationHeader(tk);
            return new NetworkCredential {UserName = cred.Item1, Password = cred.Item2};
        }
    }
}