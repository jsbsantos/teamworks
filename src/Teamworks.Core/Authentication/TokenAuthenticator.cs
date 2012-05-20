using Raven.Client;
using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Core.Authentication
{
    public sealed class TokenAuthenticator : IAuthenticator
    {
        #region IAuthenticator Members

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

            dynamic t = dyn.Token;
            if (t == null)
            {
                return false;
            }

            IDocumentSession session = Global.Raven.CurrentSession;
            Token token = session.Include<Token>(e => e.Person).Load("tokens/" + t);
            if (token == null)
            {
                return false;
            }

            person = session.Load<Person>(token.Person);
            return true;
        }

        #endregion
    }
}