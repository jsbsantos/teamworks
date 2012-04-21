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
        public Project()
        {
            PeopleReference = new List<Reference<Person>>();
            TasksReference = new List<Reference<Task>>();
        }

        public string Description { get; set; }

        public Person Owner { get; set; }
        public IList<Reference<Person>> PeopleReference { get; set; }
        public IList<Reference<Task>> TasksReference  { get; set; }
        public bool Archived { get; set; }

        public static Project Load(string id)
        {
            var project = Session
                .Include("TasksReference")
                .Include("PeopleReference")
                .Load<Project>(id);

            if (project == null)
                return null;

            return project;
        }
    }
}