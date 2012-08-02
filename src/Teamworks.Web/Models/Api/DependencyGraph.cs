using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class DependencyGraph
    {
        public List<ActivityRelation> Relations { get; set; } 
        public List<DryActivity> Elements { get; set; } 
    }

    public class ActivityRelation
    {
        public int Activity { get; set; }
        public int Parent { get; set; }
        public int Duration { get; set; }
    }
}