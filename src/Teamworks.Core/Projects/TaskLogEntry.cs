using System;
using Newtonsoft.Json;
using Teamworks.Core.People;

namespace Teamworks.Core.Projects {
    public class TaskLogEntry : Entity<TaskLogEntry> {
        private Reference<Person> _innerOwnerReference;
        public string Description { get; set; }

        public Reference<Person> OwnerReference {
            get { return (_innerOwnerReference ?? (_innerOwnerReference = new Reference<Person>())); }
            set { _innerOwnerReference = value; }
        }

        public long Duration { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public Person Owner { get; set; }

        public static TaskLogEntry Load(string id) {
            var log = Session
                .Include("OwnerReference")
                .Load<TaskLogEntry>(id);

            if (log == null) {
                return null;
            }

            if (log.OwnerReference != null) {
                log.Owner = Session.Load<Person>(log.OwnerReference.Id);
            }

            return log;
        }
    }
}