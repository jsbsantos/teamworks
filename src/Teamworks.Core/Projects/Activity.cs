using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Activity : Entity
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public IList<Timelog> Timelogs { get; set; }
        public IList<string> Dependencies { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People{ get; set; }
        public IList<TodoList> Todos { get; set; }
        
        public int LastTimeEntryId { get; private set; }
        public int GenerateNewTimeEntryId()
        {
            return ++LastTimeEntryId;
        }
        
        public int LastTodoListId { get; private set; }
        public string Name { get; set; }

        public int GenerateNewTodoListId()
        {
            return ++LastTodoListId;
        }

        public static Activity Forge(string project, string name, string description)
        {
            return new Activity
                       {
                           Name = name,
                           Project = project,
                           Description = description,
                           Dependencies = new List<string>(),
                           Discussions = new List<string>(),
                           People = new List<string>(),
                           Timelogs = new List<Timelog>(),
                           Todos = new List<TodoList>(),
                           LastTimeEntryId = 0
                       };
        }
    }
}