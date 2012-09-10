using System;
using System.Collections.Generic;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ProjectWithStatisticsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int PeopleCount { get; set; }
        public int TotalEstimatedTime { get; set; }
        public int TotalTimeLogged { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public IEnumerable<ActivitiesDuration.Result> ActivitySummary { get; set; }

        public List<ProjectViewModel.Timelog> Timelogs { get; set; }

        public int TotalTime { get; set; }
    }
}