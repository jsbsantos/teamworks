using System;
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
        public IList<Timelog> Timelogs { get; set; }

        public ProjectViewModel()
        {
            Timelogs=new List<Timelog>(); 
        }

        #region Nested type: ActivityViewModel

        public class Activity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        #endregion

        #region Nested type: TimelogViewModel

        public class Timelog
        {
            public EntityViewModel Project { get; set; }
            public EntityViewModel Activity { get; set; }
            public EntityViewModel Person { get; set; }

            public int Id { get; set; }
            public int Duration { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
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