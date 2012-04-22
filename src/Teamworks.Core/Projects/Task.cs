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
        private IList<Reference<Person>> _innerPeopleReferenceList;
        private IList<Reference<Task>> _innerPredecessorReferenceList;

        public enum TaskStatus
        {
            InProgress
        }

        public Task(string name, string description, long estimated, DateTime due, string projectId)
        {
            Name = name;
            Description = description;
            Estimated = estimated;
            Due = due;
            Project = projectId;
        }

        public string Description { get; set; }
        public TaskStatus Status { get; set; }

        public IList<Reference<Person>> PeopleReference
        {
            get { return (_innerPeopleReferenceList ?? (_innerPeopleReferenceList = new List<Reference<Person>>())); }
            set { _innerPeopleReferenceList = value; }
        }

        public IList<Reference<Task>> PredecessorReference
        {
            get { return (_innerPredecessorReferenceList ?? (_innerPredecessorReferenceList = new List<Reference<Task>>())); }
            set { _innerPredecessorReferenceList = value; }
        }

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

            if (task.PeopleReference.Count > 0)
                task.People = Session.Load<Person>(task.PeopleReference.Select(x => x.Id)).ToList();

            if (task.PredecessorReference.Count > 0)
                task.Predecessor = Session.Load<Task>(task.PredecessorReference.Select(x => x.Id)).ToList();
            return task;
        }
    }
}