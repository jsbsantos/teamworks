using System;
using System.Net;
using System.Security.Principal;

namespace Teamworks.Core.Authentication
{
    public interface IAuthenticationHandler
    {
        bool Validate(NetworkCredential credential);
        NetworkCredential GetCredentials(dynamic token);
    }
}