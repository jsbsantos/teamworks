using System.Net;

namespace Teamworks.Core.Authentication
{
    public interface IAuthenticationHandler
    {
        bool IsValid(NetworkCredential credential);
        NetworkCredential GetCredentials(dynamic token);
    }
}