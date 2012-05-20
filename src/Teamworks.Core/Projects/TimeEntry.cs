using System;

namespace Teamworks.Core.Projects
{
    public class TimeEntry
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Duration { get; set; }

        public string Person { get; set; }

        public static TimeEntry Forge(string description, DateTime date, long duration, string person)
        {
            return new TimeEntry()
                       {
                           Description = description,
                           Date = date,
                           Duration = duration,
                           Person = person,
                           Id = null
                       };
        }
    }
}