using System;
using System.Collections.Generic;

namespace Teamworks.Web.Models.Api
{
    public class Discussion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public IList<Message> Messages { get; set; }
       
    }
}