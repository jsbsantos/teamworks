using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Teamworks.Core.Entities;
using Teamworks.Core.People;
using Raven.Client.Linq;

namespace Teamworks.Core.Projects
{
    public class Project : Entity<Project>
    {
        private IList<Reference<Person>> _innerPeopleRefList;
        private IList<Reference<Task>> _innerTaskReferenceList;
        public string Description { get; set; }

        public IList<Reference<Person>> PeopleReference { get { return (_innerPeopleRefList ?? (_innerPeopleRefList = new List<Reference<Person>>())); } set { _innerPeopleRefList = value; } }
        public IList<Reference<Task>> TasksReference { get { return (_innerTaskReferenceList ?? (_innerTaskReferenceList = new List<Reference<Task>>())); } set { _innerTaskReferenceList = value; } }
        public bool Archived { get; set; }

        [JsonIgnore]
        public long TotalEstimatedHours
        {
            get { return Session.Query<Task>().Where(x => x.Project == Id).ToList().Sum(x => x.Estimated); }
        }
        [JsonIgnore]
        public long TotalConsumedHours
        {
            get { return Session.Query<Task>().Where(x => x.Project == Id).ToList().Sum(x => x.Consumed); }
        }

        [JsonIgnore]
        public IList<Person> People { get; set; }

        [JsonIgnore]
        public IList<Task> Tasks { get; set; }

        public static Project Load(string id)
        {
            var project = Session
                .Include("TasksReference")
                .Include("PeopleReference")
                .Load<Project>(id);

            if (project == null)
                return null;

            if (project.TasksReference.Count > 0)
                project.Tasks = Session.Load<Task>(project.TasksReference.Select(x => x.Id)).ToList();

            if (project.PeopleReference.Count > 0)
                project.People = Session.Load<Person>(project.PeopleReference.Select(x => x.Id)).ToList();

            return project;
        }
    }
}