using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models
{
    public class Project
    {
        public string Id { get; set; }
        
        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Description { get; set; }

        public ICollection<Task> Tasks { get; set; }
        public ICollection<Discussions> Discussions { get; set; }
    }
}