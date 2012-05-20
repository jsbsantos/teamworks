using System;
using System.Collections.Generic;

namespace Teamworks.Core.Authentication
{
    public class AuthenticatorFactory : Dictionary<string, IAuthenticator>
    {
        private static readonly Lazy<AuthenticatorFactory> _instance =
            new Lazy<AuthenticatorFactory>(() => new AuthenticatorFactory());

        public static AuthenticatorFactory Instance
        {
            get { return _instance.Value; }
        }
    }
}