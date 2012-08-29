using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ProjectsViewModel
    {
        public ProjectsViewModel()
        {
            Projects = new List<Project>();
        }

        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

        public IList<Project> Projects { get; set; }

        #region Nested type: ProjectViewModel

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

        public class Input
        {
            [Required(AllowEmptyStrings = true)]
            public int Id { get; set; }

            [Required(AllowEmptyStrings = false)]
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}