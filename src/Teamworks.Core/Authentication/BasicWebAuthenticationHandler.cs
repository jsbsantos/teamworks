using System.Net;

namespace Teamworks.Core.Authentication
{
    public class BasicWebAuthenticationHandler : WebAuthentication
    {
        public override NetworkCredential GetCredentials(dynamic token)
        {
            return new NetworkCredential(token.Username, token.Password);
        }
    }
}