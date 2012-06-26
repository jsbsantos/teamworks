using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Activity : Entity
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public IList<Timelog> Timelogs { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People{ get; set; }

        public int LastTimeEntryId { get; private set; }
        public int GenerateNewTimeEntryId()
        {
            return ++LastTimeEntryId;
        }

        public static Activity Forge(string project, string name, string description)
        {
            return new Activity
                       {
                           Name = name,
                           Project = project,
                           Description = description,
                           Discussions = new List<string>(),
                           People = new List<string>(),
                           Timelogs = new List<Timelog>(),
                           LastTimeEntryId = 0
                       };
        }
    }
}