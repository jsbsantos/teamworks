using System.Net;

namespace Teamworks.Core.Authentication {
    public interface IAuthenticationHandler {
        bool Validate(NetworkCredential credential);
        NetworkCredential GetCredentials(dynamic token);
    }
}