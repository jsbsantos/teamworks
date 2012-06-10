using System.Collections.Generic;

namespace Teamworks.Core.Projects
{
    public class Task : Entity
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public IList<string> Pretasks { get; set; }
        public IList<TimeEntry> Timelog { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People{ get; set; }

        public int LastTimeEntryId { get; private set; }
        public int GenerateNewTimeEntryId()
        {
            return ++LastTimeEntryId;
        }

        public static Task Forge(string project, string name, string description)
        {
            return new Task
                       {
                           Name = name,
                           Project = project,
                           Description = description,
                           Pretasks = new List<string>(),
                           Discussions = new List<string>(),
                           People = new List<string>(),
                           Timelog = new List<TimeEntry>(),
                           LastTimeEntryId = 0
                       };
        }
    }
}