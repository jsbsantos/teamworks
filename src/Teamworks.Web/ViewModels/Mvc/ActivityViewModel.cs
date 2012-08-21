using System;
using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        public EntityViewModel ProjectReference { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int TotalTimeLogged { get; set; }
        public DateTimeOffset StartDate { get; set; }

        #region Nested Type: Input

        public class Input
        {
            public int Id { get; set; }
            public int Project { get; set; }
            
            public string Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public DateTimeOffset StartDate { get; set; }

            public IEnumerable<int> Dependencies { get; set; }
        } 
        #endregion
    }

    public class DependencyActivityViewModel : ActivityViewModel
    {
        public bool Dependency { get; set; }
    }

    public class ActivityViewModelComplete : ActivityViewModel
    {
        public IEnumerable<TimelogViewModel> Timelogs { get; set; }
        public IEnumerable<DiscussionViewModel> Discussions { get; set; } 
        public IEnumerable<DependencyActivityViewModel> Dependencies { get; set; }
        public IEnumerable<AssignedPersonViewModel> People { get; set; }

        public class AssignedPersonViewModel : PersonViewModel
        {
            public bool Assigned { get; set; }
        }
    }
}