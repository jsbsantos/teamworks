using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        [Required]
        public int Project { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }

        public int Duration { get; set; }
    }

    public class CompleteActivityViewModel : ActivityViewModel
    {
        public IList<int> People { get; set; }
        public IList<int> Dependencies { get; set; }
    }
}