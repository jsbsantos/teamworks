using System;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public long Duration { get; set; }
        public DryPerson Person { get; set; }
    }
}