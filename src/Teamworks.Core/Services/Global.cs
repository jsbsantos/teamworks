using System.Threading;
using System.Security.Principal;
using Raven.Client;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services.RavenDb;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static Executor Executor { get; set; }

        public static AuthenticatorFactory Authentication
        {
            get { return AuthenticatorFactory.Instance; }
        }

        public static IDocumentStore Database;

        public static Person CurrentPerson
        {
            get
            {
                var principal = Thread.CurrentPrincipal;
                if (principal == null)
                {
                    throw new IdentityNotMappedException("Anonymous access.");
                }

                var person = principal.Identity as PersonIdentity;
                if (person == null)
                    throw new IdentityNotMappedException("No identity set.");

                return person.Person;
            }

        }

        public static class Constants
        {
            public const string Operation = "GOD";
        }
    }
}