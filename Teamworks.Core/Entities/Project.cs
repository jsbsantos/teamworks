using System.Collections.Generic;

namespace Teamworks.Core.Entities
{
    public class Project : Entity
    {
        public string Description { get; set; }
        public User Owner { get; set; }
        public IList<Reference<User>> People { get; set; }
        public IList<Reference<Task>> Tasks { get; set; }
        public bool Archived { get; set; }
    }
}