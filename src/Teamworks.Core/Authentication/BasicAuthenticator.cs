using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Core.Authentication
{
    public sealed class BasicAuthenticator : IAuthenticator
    {
        public bool IsValid(object dyn)
        {
            Person person;
            return IsValid(dyn, out person);
        }

        public bool IsValid(object obj, out Person person)
        {
            person = null;
            var dyn = obj as dynamic;
            if (dyn == null)
            {
                return false;
            }

            var username = dyn.Username;
            var password = dyn.Password;

            Person p = Global.Raven.CurrentSession.Load<Person>("people/" + username);
            if (p == null)
            {
                return false;
            }

            person = p;
            return p.IsThePassword(password);
        }
    }
}