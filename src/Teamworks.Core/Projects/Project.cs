using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Teamworks.Core.People;

namespace Teamworks.Core.Projects {
    public class Project : Entity<Project> {
        private IList<Reference<Person>> _innerPeopleRefList;
        private IList<Reference<Task>> _innerTaskReferenceList;

        public Project(string name, string description) {
            PeopleReference = new List<Reference<Person>>();
            TasksReference = new List<Reference<Task>>();

            Name = name;
            Description = description;
        }

        public string Description { get; set; }

        public IList<Reference<Person>> PeopleReference {
            get { return (_innerPeopleRefList ?? (_innerPeopleRefList = new List<Reference<Person>>())); }
            set { _innerPeopleRefList = value; }
        }

        public IList<Reference<Task>> TasksReference {
            get { return (_innerTaskReferenceList ?? (_innerTaskReferenceList = new List<Reference<Task>>())); }
            set { _innerTaskReferenceList = value; }
        }

        public bool Archived { get; set; }

        [JsonIgnore]
        public long TotalEstimatedHours {
            //get { return Session.Query<Task>().Where(x => x.Project == Id).ToList().Sum(x => x.Estimated); }
            get { return Tasks.Sum(x => x.Estimated); }
        }

        [JsonIgnore]
        public long TotalConsumedHours {
            //get { return Session.Query<Task>().Where(x => x.Project == Id).ToList().Sum(x => x.Consumed); }
            get { return Tasks.Sum(x => x.Consumed); }
        }

        [JsonIgnore]
        public IList<Person> People { get; set; }

        [JsonIgnore]
        public IList<Task> Tasks { get; set; }

        public static Project Load(string id) {
            var project = Session
                .Include("TasksReference")
                .Include("PeopleReference")
                .Load<Project>(id);

            if (project == null) {
                return null;
            }

            if (project.TasksReference.Count > 0) {
                project.Tasks = Session.Load<Task>(project.TasksReference.Select(x => x.Id)).ToList();
            }

            if (project.PeopleReference.Count > 0) {
                project.People = Session.Load<Person>(project.PeopleReference.Select(x => x.Id)).ToList();
            }

            return project;
        }
    }
}