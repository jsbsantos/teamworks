using System;
using System.Collections.Generic;
using System.Linq;

namespace Teamworks.Core
{
    public class Activity : Entity
    {
        public string Name { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public IList<Timelog> Timelogs { get; set; }
        public IList<string> Dependencies { get; set; }
        public IList<string> Discussions { get; set; }
        public IList<string> People { get; set; }
        public IList<TodoList> Todos { get; set; }
        public DateTime StartDate { get; set; }

        public int LastTimeEntryId { get; private set; }

        public int LastTodoListId { get; private set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastTimeEntryId;
        }

        public int GenerateNewTodoListId()
        {
            return ++LastTodoListId;
        }

        public static Activity Forge(string project, string name, string description, int duration)
        {
            return new Activity
                       {
                           Name = name,
                           Project = project,
                           Description = description ?? "",
                           Duration = duration,
                           Dependencies = new List<string>(),
                           Discussions = new List<string>(),
                           People = new List<string>(),
                           Timelogs = new List<Timelog>(),
                           Todos = new List<TodoList>(),
                           LastTimeEntryId = 0
                       };
        }

        public IEnumerable<ActivityRelation> DependencyGraph(IEnumerable<Activity> parents)
        {
            return Dependencies.Select(p =>
                                      {
                                          Activity parent = parents.Single(x => p.Equals(x.Id,
                                                                                         StringComparison.
                                                                                             InvariantCultureIgnoreCase));
                                          return new ActivityRelation
                                                     {
                                                         Parent = parent.Identifier,
                                                         Activity = Identifier,
                                                         Duration = parent.Duration,
                                                     };
                                      }).ToList();
        }
    }
}