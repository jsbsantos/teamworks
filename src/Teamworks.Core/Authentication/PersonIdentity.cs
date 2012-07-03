using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Teamworks.Core.Authentication
{
    [Serializable]
    public class PersonIdentity : IIdentity, ISerializable
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.State != StreamingContextStates.CrossAppDomain)
            {
                throw new InvalidOperationException("Serialization not supported");
            }

            var identity = new GenericIdentity(Name, AuthenticationType);
            info.SetType(identity.GetType());

            MemberInfo[] members = FormatterServices.GetSerializableMembers(identity.GetType());
            object[] values = FormatterServices.GetObjectData(identity, members);
            for (var i = 0; i < members.Length; i++)
            {
                info.AddValue(members[i].Name, values[i]);
            }
        }
    }
}