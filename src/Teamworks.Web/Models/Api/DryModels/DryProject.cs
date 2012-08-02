using System;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api.DryModels
{
    public class DryProject
    {
        public string Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 6)]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
    }
}