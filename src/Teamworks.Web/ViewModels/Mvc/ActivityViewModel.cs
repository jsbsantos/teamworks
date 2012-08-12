using System;
using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
{

    public class ActivityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
    }


    public class ActivityViewModelComplete : ActivityViewModel
    {
        public EntityViewModel ProjectReference { get; set; }

        public IEnumerable<TimelogViewModel> Timelogs { get; set; }
        public IEnumerable<ActivityViewModel> Dependencies { get; set; }
        public IEnumerable<PersonViewModel> AssignedPeople { get; set; }
    }

    public class TimelogViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }

        public PersonViewModel Person { get; set; }
    }
}