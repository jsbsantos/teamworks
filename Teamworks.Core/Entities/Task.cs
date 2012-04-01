using System;
using System.Collections.Generic;

namespace Teamworks.Core.Entities
{
    public class Task : Entity
    {
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public IList<Reference<User>> People { get; set; }
        public IList<Reference<Task>> Predecessor { get; set; }
        public long Estimated { get; set; }
        public long Consumed { get; set; }
        public DateTime Due { get; set; }

    }
}