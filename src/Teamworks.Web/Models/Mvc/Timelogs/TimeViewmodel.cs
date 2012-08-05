using System.Collections.Generic;

namespace Teamworks.Web.Models.Mvc.Timelogs
{
    public class TimeViewModel
    {
        public IEnumerable<TimeTypeahead> Source { set; get; }
    }

    public class TimeTypeahead
    {
        public int ActivityId { get; set; }
        public string Activity { get; set; }
        public int ProjectId { get; set; }
        public string Project { get; set; }
    }
}