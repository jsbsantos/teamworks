﻿using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class TimelogViewModel
    {
        public Core.Project Project;
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public int Duration { get; set; }

        public Core.Activity Activity { get; set; }
    }
}