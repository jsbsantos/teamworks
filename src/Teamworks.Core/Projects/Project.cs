using System.Collections.Generic;

namespace Teamworks.Core.Projects
{
    public class Project : Entity
    {
        public string Description { get; set; }
        public IList<string> Tasks { get; set; }
        public bool Archived { get; set; }
        public IList<string> People { get; set; }
        public IList<string> Threads { get; set; }
        
        public static Project Forge(string name, string description)
        {
            return new Project
                       {
                           Name = name,
                           Description = description,
                           Tasks = new List<string>(),
                           Threads = new List<string>(),
                           People = new List<string>(),
                       };
        }

    }
}