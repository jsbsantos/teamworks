using System;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 1)]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}