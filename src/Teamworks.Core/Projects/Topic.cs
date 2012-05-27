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
        public string Project { get; set; }

        public int LastDiscussionId { get; private set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastDiscussionId;
        }

        public static Topic Forge(string name, string text, string project, string person)
        {
            return new Topic()
                       {
                           Text = text,
                           Name = name,
                           Date = DateTime.Now,
                           Messages = new List<Message>(),
                           Person = person,
                           LastDiscussionId = 0,
                           Project = project
                       };
        }
    }

    //public class TopicMessage : Message
    //{
    //    public IList<Message> Replies { get; set; }
    //}
}