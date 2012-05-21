using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models
{
    public class TaskModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(512,MinimumLength = 5)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }

        //public DateTime Deadline { get; set; }
        //public long EstimatedTime { get; set; }

        public ICollection<TimeEntryModel> Timelog { get; set; }

    }
}