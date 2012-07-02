using Teamworks.Core.Authentication;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static AuthenticatorFactory Authentication
        {
            get { return AuthenticatorFactory.Instance; }
        }

        public static RavenDB Database
        {
            get { return RavenDB.Instance; }
        }
    }
}