using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ProjectsViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

        public IList<Project> Projects { get; set; }

        #region Nested type: Project

        public class Project
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int NumberOfActivities { get; set; }
            public int NumberOfDiscussions { get; set; }
            public IList<PersonViewModel> People { get; set; }
        }

        #endregion
    }
}