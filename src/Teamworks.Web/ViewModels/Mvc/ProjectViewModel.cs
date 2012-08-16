using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PersonViewModel> People { get; set; }
        public IList<Activity> Activities { get; set; }
        public IList<Discussion> Discussions { get; set; }

        #region Nested type: ActivityViewModel

        public class Activity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        #endregion

        #region Nested type: Discussion

        public class Discussion
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
        }

        #endregion

        
    }
}