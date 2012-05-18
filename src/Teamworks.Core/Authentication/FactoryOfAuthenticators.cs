using System;
using System.Collections.Generic;

namespace Teamworks.Core.Authentication
{
    public class FactoryOfAuthenticators : Dictionary<string, IAuthenticator>
    {
        private static readonly Lazy<FactoryOfAuthenticators> _instance =
            new Lazy<FactoryOfAuthenticators>(() => new FactoryOfAuthenticators());

        public static FactoryOfAuthenticators Instance
        {
            get { return _instance.Value; }
        }
    }
}