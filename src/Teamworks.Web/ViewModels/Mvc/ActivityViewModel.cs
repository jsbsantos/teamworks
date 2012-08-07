using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public Project ProjectReference { get; set; }
        public IList<Timelog> Timelogs { get; set; }

        public class Project
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public class Timelog
        {
            
        }
    }
}