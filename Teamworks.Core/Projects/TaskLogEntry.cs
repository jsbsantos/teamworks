using System;
using System.Linq;
using Newtonsoft.Json;
using Teamworks.Core.Entities;
using Teamworks.Core.People;

namespace Teamworks.Core.Projects
{
    public class TaskLogEntry : Entity<TaskLogEntry>
    {
        public string Description { get; set; }
        public Reference<Person> OwnerReference { get; set; }
        public long Duration { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Person Owner { get; set; }

        public static TaskLogEntry Load(string id)
        {
            var log = Session
                .Include("OwnerReference")
                .Load<TaskLogEntry>(id);

            if (log == null)
                return null;

            log.Owner = Session.Load<Person>(log.OwnerReference.Id);
            return log;
        }

    }
}