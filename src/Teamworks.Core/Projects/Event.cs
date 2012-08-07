using System;
using System.Collections.Generic;

namespace Teamworks.Core
{
    public class Event : Entity
    {
        public string Description { get; set; }
        public string Host { get; set; }
        public IList<string> Attendees { get; set; }
        public DateTime Schedule { get; set; }
        public string Name { get; set; }

        public static Event Forge(string name, string description, string host, DateTime schedule)
        {
            return new Event
                       {
                           Name = name,
                           Attendees = new List<string>(),
                           Description = description,
                           Host = host,
                           Schedule = schedule
                       };
        }
    }
}