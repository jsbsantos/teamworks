using System.Collections.Generic;

namespace Teamworks.Core.Projects {
    public class Project : Entity<Project> {
        public Project(string name, string description) {
            Name = name;
            Description = description;
            TaskIds = new List<string>();
        }

        public string Description { get; set; }
        public IList<string> TaskIds { get; set; }
        public bool Archived { get; set; }
    }
}