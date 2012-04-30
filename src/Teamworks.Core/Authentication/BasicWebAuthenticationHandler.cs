using System.Net;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    public class BasicWebAuthenticationHandler : IAuthenticationHandler
    {
        public bool Validate(NetworkCredential credential)
        {
            return Person.Authenticate(credential.UserName, credential.Password);
        }

        public NetworkCredential GetCredentials(dynamic token)
        {
            return new NetworkCredential(token.Username, token.Password);
        }
    }
}