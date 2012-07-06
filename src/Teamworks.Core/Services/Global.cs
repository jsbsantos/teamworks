using Teamworks.Core.Authentication;
using Teamworks.Core.Services.RavenDb;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static AuthenticatorFactory Authentication
        {
            get { return AuthenticatorFactory.Instance; }
        }

        public static Session Database
        {
            get { return Session.Instance; }
        }
    }
}