
using System.Collections.Generic;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Core
{
    public class DependencyGraph
    {
        public IEnumerable<ActivityRelation> Relations { get; set; }
        public IEnumerable<Activities_Duration> Elements { get; set; } 
    }

    public class ActivityRelation
    {
        public int Activity { get; set; }
        public int Parent { get; set; }
        public int Duration { get; set; }
    }
}