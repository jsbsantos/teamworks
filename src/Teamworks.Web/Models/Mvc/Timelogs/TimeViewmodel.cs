using System.Collections.Generic;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Models.Mvc.Timelogs
{
    public class TimeViewModel
    {
        public IEnumerable<TimeTypeahead> Source { set; get; }
        public List<Timelog> Timelogs { get; set; }
    }

    public class TimeTypeahead
    {
        public int ActivityId { get; set; }
        public string Activity { get; set; }
        public int ProjectId { get; set; }
        public string Project { get; set; }
    }
}