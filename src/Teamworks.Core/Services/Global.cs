using System.Security.Principal;
using System.Threading;
using Teamworks.Core.Authentication;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static AuthenticatorFactory Authentication
        {
            get { return AuthenticatorFactory.Instance; }
        }

        public static RavenDb Database
        {
            get { return RavenDb.Instance; }
        }


        //exception are used because anonymous should never access this
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
    }
}