using System;
using System.Collections.Generic;
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
        public IList<string> People { get; set; }
        public IList<Todo> Todos { get; set; }
        public DateTimeOffset StartDateConsecutive { get; set; }
        public DateTimeOffset StartDate { get; set; }

        public int LastTimeEntryId { get; private set; }

        public int LastTodoId { get; private set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastTimeEntryId;
        }

        public int GenerateNewTodoId()
        {
            return ++LastTodoId;
        }

        public static Activity Forge(int project, string name, string description, int duration,
                                     DateTimeOffset startDate = new DateTimeOffset())
        {
            return new Activity
                {
                    Name = name,
                    Project = project.ToId("project"),
                    Description = description ?? "",
                    Duration = duration,
                    Dependencies = new List<string>(),
                    People = new List<string>(),
                    Timelogs = new List<Timelog>(),
                    Todos = new List<Todo>(),
                    LastTimeEntryId = 0,
                    StartDateConsecutive = startDate == DateTimeOffset.MinValue ? DateTimeOffset.Now : startDate,
                    StartDate = startDate == DateTimeOffset.MinValue ? DateTimeOffset.Now : startDate,
                };
        }
    }
}