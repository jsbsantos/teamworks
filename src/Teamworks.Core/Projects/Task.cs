using System.Collections.Generic;

namespace Teamworks.Core.Projects {
    public class Task : Entity {
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public IList<string> PreTaskIds { get; set; }

        public static Task Forge(string name, string description) {
            return new Task
                   {
                       Name = name,
                       Description = description,
                       PreTaskIds = new List<string>()
                   };
        }
    }
}