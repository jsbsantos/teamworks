using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api
{
    public class Project
    {
        public string Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Description { get; set; }

        public ICollection<Person> People { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Discussion> Discussions { get; set; }

        public string Token { get; set; }
    }
}