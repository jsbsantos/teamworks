using System;
using System.Net;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication {
    public class BasicAuthenticationHandler : IAuthenticationHandler {
        #region IAuthenticationHandler Members

        public bool Validate(NetworkCredential credential) {
            return Person.Authenticate(credential.UserName, credential.Password);
        }

        public NetworkCredential GetCredentials(dynamic token) {
            var tk = token as string;
            if (String.IsNullOrWhiteSpace(tk)) {
                throw new ArgumentNullException("token");
            }
            Tuple<string, string> cred = Global.DecodeBasicAuthenticationHeader(tk);
            return new NetworkCredential {UserName = cred.Item1, Password = cred.Item2};
        }

        #endregion
    }
}