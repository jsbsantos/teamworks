using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class ActivitiesTotalDuration : AbstractIndexCreationTask<Activity, ActivitiesTotalDuration.Result>
    {
        public ActivitiesTotalDuration()
        {
            Map = activities => from act in activities
                                from timelog in act.Timelogs
                                select new
                                           {
                                               ActivityId = act.Id,
                                               timelog.Duration
                                           };

            Reduce = activities => from act in activities
                                   group act by act.ActivityId
                                   into grouping
                                   select new
                                              {
                                                  ActivityId = grouping.Key,
                                                  Duration = grouping.Sum(s => s.Duration)
                                              };

            Index(x => x.ActivityId, FieldIndexing.Analyzed);
        }

        #region Nested type: Result

        public class Result
        {
            public string ActivityId { get; set; }
            public int Duration { get; set; }
        }

        #endregion
    }
}