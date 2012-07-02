using System;
using System.ComponentModel.DataAnnotations;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Models.Api
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        [StringLength(1024, MinimumLength = 1)]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DryPerson Person { get; set; }
    }
}