using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    public interface IAuthenticator
    {
        bool IsValid(object dyn);
        bool IsValid(object dyn, out Person person);
    }
}