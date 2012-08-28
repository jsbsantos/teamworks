using System;

namespace Teamworks.Core
{
    public class Timelog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Duration { get; set; }

        public string Person { get; set; }

        public static Timelog Forge(string description, int duration, DateTimeOffset date, string person)
        {
            return new Timelog
                       {
                           Description = description,
                           Date = date,
                           Duration = duration,
                           Person = person,
                           Id = 0
                       };
        }
    }
}