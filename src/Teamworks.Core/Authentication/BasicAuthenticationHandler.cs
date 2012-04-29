using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using Raven.Client.Linq;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    public class BasicAuthenticationHandler : IAuthenticationHandler
    {
        public bool Validate(NetworkCredential credential)
        {
            return Person.Authenticate(credential.UserName, credential.Password);
        }
        public NetworkCredential GetCredentials(dynamic token)
        {
            string tk = token as string;
            if (String.IsNullOrWhiteSpace(tk))
            {
                throw new ArgumentNullException("token");
            }
            var cred = Global.DecodeBasicAuthenticationHeader(tk);
            return new NetworkCredential() { UserName = cred.Item1, Password = cred.Item2 };
        }
    }
}