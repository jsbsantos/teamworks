using System;
using System.Collections.Generic;
using System.Linq;
using Teamworks.Core.Services;

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
        public DateTimeOffset StartDate { get; set; }

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

        public static Activity Forge(int project, string name, string description, int duration, DateTimeOffset startDate = new DateTimeOffset())
        {
            return new Activity
                       {
                           Name = name,
                           Project = project.ToId("project"),
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


        public static double GetAccumulatedDuration(ICollection<Activity> domain, Activity activity, int duration = 0)
        {
            var parent = domain.Where(a => activity.Dependencies.Contains(a.Id))
                .OrderByDescending(a => a.Duration).FirstOrDefault();

            if (parent == null)
                return duration;

            return GetAccumulatedDuration(domain, parent, duration + parent.Duration);
        }

    }
}