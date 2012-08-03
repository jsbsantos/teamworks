using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class Activities_Duration : AbstractIndexCreationTask<Activity, Activities_Duration.Result>
    {
        public class Result
        {
            public string Activity { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public DateTime StartDate { get; set; }
            public string Project { get; set; }
            public int TimeUsed { get; set; }
            public List<string> Dependencies { get; set; }
        }

        public Activities_Duration()
        {
            Map = activities => from act in activities
                                select new
                                           {
                                               TimeUsed = act.Timelogs.Sum(x=>x.Duration),
                                               Activity = act.Id,
                                               Project = act.Project,
                                               Duration = act.Duration,
                                               StartDate = act.StartDate,
                                               Name = act.Name,
                                               Description = act.Description,
                                               Dependencies = act.Dependencies
                                           };

            Reduce = activities => from act in activities
                                   group act by act.Activity
                                   into g
                                   select new
                                              {
                                                  TimeUsed = g.Select(p => p.TimeUsed).First(),
                                                  Activity = g.Key,
                                                  Project = g.Select(p => p.Project).First(),
                                                  Duration = g.Select(p => p.Duration).First(),
                                                  StartDate = g.Select(p => p.StartDate).First(),
                                                  Name = g.Select(p => p.Name).First(),
                                                  Description = g.Select(p => p.Description).First(),
                                                  Dependencies = g.Select(p => p.Dependencies).FirstOrDefault()
                                              };

            Index(x => x.Activity, FieldIndexing.Analyzed);
        }
    }
}