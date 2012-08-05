using System;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.Models.Api
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public string Token { get; set; }
    }
}