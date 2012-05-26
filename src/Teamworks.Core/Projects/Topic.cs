using System;
using System.Collections.Generic;

namespace Teamworks.Core.Projects
{
    public class Topic : Entity
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public IList<Message> Messages { get; set; }
    }

    public class Message
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
    }

    //public class TopicMessage : Message
    //{
    //    public IList<Message> Replies { get; set; }
    //}

}