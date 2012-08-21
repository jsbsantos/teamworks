using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gravatar { get; set; }

        public class PersonEqualityComparer : IEqualityComparer<PersonViewModel>
        {
            public bool Equals(PersonViewModel x, PersonViewModel y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(PersonViewModel obj)
            {
                return RuntimeHelpers.GetHashCode((object) obj);
            }

        }
    }
}