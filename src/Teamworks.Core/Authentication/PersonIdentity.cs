using System.Security.Principal;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication {
    public class PersonIdentity : IIdentity {

        public PersonIdentity(Person person) {
            Person = person;
        }

        public Person Person { get; private set; }

        #region Implementation of IIdentity

        public string Name {
            get { return Person.Username; }
        }
        
        public string AuthenticationType { get; set; }
        
        public bool IsAuthenticated {
            get { return true; }
        }

        #endregion
    }
}