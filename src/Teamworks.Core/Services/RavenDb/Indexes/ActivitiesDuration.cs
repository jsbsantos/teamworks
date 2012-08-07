using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ActivitiesDuration : AbstractIndexCreationTask<Activity, ActivitiesDuration.Result>
    {
        public ActivitiesDuration()
        {
            Map = activities => from act in activities
                                select new
                                           {
                                               TimeUsed = act.Timelogs.Sum(x => x.Duration),
                                               act.Id,
                                               act.Project,
                                               act.Duration,
                                               act.StartDate,
                                               act.Name,
                                               act.Description, Dependencies = act.Dependencies
                                           };

            Reduce = activities => from act in activities
                                   group act by act.Id
                                   into g
                                   select new
                                              {
                                                  TimeUsed = g.Select(p => p.TimeUsed).First(),
                                                  Id = g.Key,
                                                  Project = g.Select(p => p.Project).First(),
                                                  Duration = g.Select(p => p.Duration).First(),
                                                  StartDate = g.Select(p => p.StartDate).First(),
                                                  Name = g.Select(p => p.Name).First(),
                                                  Description = g.Select(p => p.Description).First(),
                                                  Dependencies = g.Select(p => p.Dependencies).FirstOrDefault()
                                              };

            Index(x => x.Id, FieldIndexing.Analyzed);
        }

        #region Nested type: Result

        public class Result : Activity
        {
            public int TimeUsed { get; set; }
        }

        #endregion
    }
}