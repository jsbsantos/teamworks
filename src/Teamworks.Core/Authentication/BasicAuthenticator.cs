using Teamworks.Core.Services;

namespace Teamworks.Core.Authentication
{
    public sealed class BasicAuthenticator : IAuthenticator
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
            return dyn != null && IsValid(dyn.Username, dyn.Password, out person);
        }

        #endregion

        public bool IsValid(string username, string password)
        {
            Person person;
            return IsValid(username, password, out person);
        }

        public bool IsValid(string username, string password, out Person person)
        {
            person = null;
            var p = Global.Database.CurrentSession.Load<Person>("people/" + username);
            if (p == null)
            {
                return false;
            }

            person = p;
            return p.IsThePassword(password);
        }
    }
}