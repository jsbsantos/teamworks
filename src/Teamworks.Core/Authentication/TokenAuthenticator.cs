using Teamworks.Core.People;

namespace Teamworks.Core.Authentication
{
    public sealed class TokenAuthenticator : IAuthenticator
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
            
            var t = dyn.Token;
            if (t == null)
            {
                return false;
            }

            var session = Global.Raven.CurrentSession;
            Token token = session.Include<Token>(e => e.Person).Load("tokens/" + t);
            if (token == null)
            {
                return false;
            }

            person = session.Load<Person>(token.Person);
            return true;

        }
    }
}