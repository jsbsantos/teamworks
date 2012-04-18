using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        public IList<Reference<Person>> PeopleReference { get; set; }
        public IList<Reference<Task>> PredecessorReference { get; set; }
        public long Estimated { get; set; }
        public long Consumed { get; set; }
        public DateTime Due { get; set; }
        public IList<TaskLogEntry> Log { get; set; }
        public string Project { get; set; }

        [JsonIgnore]
        public IList<Task> Predecessor { get; set; }

        [JsonIgnore]
        public IList<Person> People { get; set; }

        public static Task Load(string id)
        {
            var task = Session
                .Include("PeopleReference")
                .Include("PredecessorReference")
                .Load<Task>(id);

            if (task == null)
                return null;

            task.People = Session.Load<Person>(task.PeopleReference.Select(x => x.Id)).ToList();
            task.Predecessor = Session.Load<Task>(task.PredecessorReference.Select(x => x.Id)).ToList();
            return task;
        }

    }
}