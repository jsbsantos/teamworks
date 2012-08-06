using System.Collections.Generic;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ProjectViewModel
    {
        public ProjectSummary Summary { get; set; }


        public List<PersonViewModel> People { get; set; }
        public IList<Activity> Activities { get; set; }
        public IList<Discussion> Discussions { get; set; }

        #region Nested type: Activity

        public class Activity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        #endregion

        #region Nested type: Discussion

        public class Discussion
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        #endregion

        #region Nested type: ProjectSummary

        public class ProjectSummary
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        #endregion
    }
}