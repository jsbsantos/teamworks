using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teamworks.Web.ViewModels.Api
{
    public class DiscussionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        public IList<Message> Messages { get; set; }
        
        public class Message
        {
            public int Id { get; set; }
            [Required]
            [StringLength(1024, MinimumLength = 1)]
            public string Content { get; set; }
            public DateTimeOffset Date { get; set; }

            
        }

        public class Input
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
    }
}