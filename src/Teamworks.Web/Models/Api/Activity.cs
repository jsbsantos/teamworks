using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api
{
    public class Activity
    {
        public string Token { get; set; }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Project { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }

        public IList<string> Dependencies { get; set; }
    }
}