using System;
using System.Collections.Generic;
using Teamworks.Core.Entities;
using Teamworks.Core.People;

namespace Teamworks.Core.Projects
{
    public class Task : Entity<Task>
    {

        public enum TaskStatus
        {
            InProgress
        }

        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public IList<Reference<Person>> People { get; set; }
        public IList<Reference<Task>> Predecessor { get; set; }
        public long Estimated { get; set; }
        public long Consumed { get; set; }
        public DateTime Due { get; set; }
        public IList<TaskLog> Log { get; set; }
        public string Project { get; set; }
    }
}