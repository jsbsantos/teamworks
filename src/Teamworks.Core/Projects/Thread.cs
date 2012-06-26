using System;
using System.Collections.Generic;

namespace Teamworks.Core.Projects
{
    public class Thread : Entity
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public IList<Message> Messages { get; set; }
        public string Entity { get; set; }
        public IList<string> Subscribers { get; set; } 

        public int LastThreadId { get; private set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastThreadId;
        }

        public static Thread Forge(string name, string text, string entity, string person)
        {
            return new Thread()
                       {
                           Text = text,
                           Name = name,
                           Date = DateTime.Now,
                           Messages = new List<Message>(),
                           Subscribers = new List<string>(),
                           Person = person,
                           LastThreadId = 0,
                           Entity = entity,
                       };
        }
    }
}