using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class Timelog_Filter : AbstractIndexCreationTask<Activity, Timelog_Filter.Result>
    {
        public Timelog_Filter()
        {
            Map = activities => from act in activities
                                from timelog in act.Timelogs
                                select new
                                           {
                                               act.Project,
                                               Activity = act.Id,
                                               timelog.Person,
                                               Timelog = timelog.Id,
                                               timelog.Date,
                                               timelog.Description,
                                               timelog.Duration,
                                               ActivityDependencies = act.Dependencies,
                                               TotalTimeLogged = timelog.Duration
                                           };

            TransformResults = (database, results) =>
                               from result in results
                               select new
                                          {
                                              result.Project,
                                              result.Activity,
                                              result.Person,
                                              result.Timelog,
                                              result.Date,
                                              result.Description,
                                              result.Duration,
                                              result.ActivityDependencies
                                          };

            Store(r => r.Timelog, FieldStorage.Yes);
            Store(r => r.Duration, FieldStorage.Yes);
            Store(r => r.Date, FieldStorage.Yes);
            Store(r => r.Description, FieldStorage.Yes);
            Store(r => r.Project, FieldStorage.Yes);
            Store(r => r.Activity, FieldStorage.Yes);
            Store(r => r.Person, FieldStorage.Yes);
            Store(r => r.ActivityDependencies, FieldStorage.Yes);

            Index(x => x.Activity, FieldIndexing.NotAnalyzed);
            Index(x => x.Person, FieldIndexing.NotAnalyzed);
            Index(x => x.Project, FieldIndexing.NotAnalyzed);
            Index(x => x.Date, FieldIndexing.NotAnalyzed);
        }

        #region Nested type: Result

        public class Result
        {
            public string Project { get; set; }
            public string Activity { get; set; }
            public IList<string> ActivityDependencies { get; set; }
            public string Person { get; set; }

            public int Timelog { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
            public int Duration { get; set; }
        }

        #endregion
    }
}