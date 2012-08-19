using System;
using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Mvc
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
            public string Content { get; set; }
            public DateTimeOffset Date { get; set; }

            public PersonViewModel Person { get; set; }
            public bool Editable { get; set; }
        }

        public class Input
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
    }
}