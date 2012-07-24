using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class DependencyGraph
    {
        public List<int[]> Relations { get; set; } 
        public List<DryActivity> Elements { get; set; } 
    }
}