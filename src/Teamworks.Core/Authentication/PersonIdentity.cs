using System;
using System.Security.Principal;

namespace Teamworks.Core.Authentication
{
    [Serializable]
    public class PersonIdentity : IIdentity
    {
        public PersonIdentity(Person person)
        {
            Person = person;
        }

        public Person Person { get; private set; }

        #region Implementation of IIdentity

        public string Name
        {
            get { return Person.Username; }
        }

        public string AuthenticationType 
        { 
            get { return "tw"; } 
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        #endregion
    }
}