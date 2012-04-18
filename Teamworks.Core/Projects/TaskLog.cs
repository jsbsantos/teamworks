using System;
using Teamworks.Core.Entities;
using Teamworks.Core.People;

namespace Teamworks.Core.Projects
{
    public class TaskLogEntry : Entity<TaskLogEntry>
    {
        public string Description { get; set; }
        public Reference<Person> Owner { get; set; }
        public long Duration { get; set; }
        public DateTime Date { get; set; }
    }
}