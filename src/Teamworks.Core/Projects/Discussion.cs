using System;
using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Discussion : Entity
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public IList<Message> Discussions { get; set; }
        public string Entity { get; set; }
        public IList<string> Subscribers { get; set; } 

        public int LastDiscussionId { get; private set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastDiscussionId;
        }

        public static Discussion Forge(string name, string content, string entity, string person)
        {
            return new Discussion()
                       {
                           Name = name,
                           Content = content,
                           Date = DateTime.Now,
                           Discussions = new List<Message>(),
                           Subscribers = new List<string>(){person},
                           Person = person,
                           LastDiscussionId = 0,
                           Entity = entity
                       };
        }
    }
}