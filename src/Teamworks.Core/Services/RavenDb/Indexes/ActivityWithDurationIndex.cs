using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ActivityWithDurationIndex : AbstractIndexCreationTask<Activity, ActivityWithDuration>
    {
        public ActivityWithDurationIndex()
        {
            Map = activities => from act in activities
                                from timelog in act.Timelogs
                                select new
                                           {
                                               TimeUsed = timelog.Duration,
                                               Activity = act.Id,
                                               Duration = act.Duration,
                                               StartDate = act.StartDate,
                                               Project = act.Project,
                                               Name = act.Name,
                                               Description = act.Description,
                                           };

            Reduce = activities => from act in activities
                                   group act by act.Activity
                                   into g
                                   select new
                                              {
                                                  Activity = g.Key,
                                                  Duration = g.Select(p => p.Duration).First(),
                                                  StartDate = g.Select(p => p.StartDate).First(),
                                                  Project = g.Select(p => p.Project).First(),
                                                  Name = g.Select(p => p.Name).First(),
                                                  Description = g.Select(p => p.Description).First(),
                                                  TimeUsed = g.Sum(x => x.TimeUsed)
                                              };

            Index(x => x.Activity, FieldIndexing.Analyzed);
        }
    }

    public class ActivityWithDuration
    {
        public string Activity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public string Project { get; set; }
        public int TimeUsed { get; set; }
    }
}