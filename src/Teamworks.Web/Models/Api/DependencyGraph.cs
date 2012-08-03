using System.Collections.Generic;

namespace Teamworks.Web.Models.Api
{
    public class DependencyGraph
    {
        public List<ActivityRelation> Relations { get; set; } 
        public List<Activity> Elements { get; set; } 
    }

    public class ActivityRelation
    {
        public int Activity { get; set; }
        public int Parent { get; set; }
        public int Duration { get; set; }
    }
}