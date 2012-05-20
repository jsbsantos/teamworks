using System.Collections.Generic;

namespace Teamworks.Core.Projects
{
    public class Task : Entity
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public IList<string> Pretasks { get; set; }
        public IList<Timelog> Timelog { get; set; }

        public static Task Forge(string project, string name, string description)
        {
            return new Task
                       {
                           Name = name,
                           Project = project,
                           Description = description,
                           Pretasks = new List<string>()
                       };
        }
    }
}