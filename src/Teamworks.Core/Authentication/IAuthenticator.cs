using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    public interface IAuthenticator
    {
        bool IsValid(object obj);
        bool IsValid(object obj, out Person person);
    }
}