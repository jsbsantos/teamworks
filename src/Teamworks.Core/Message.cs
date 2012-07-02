using System;

namespace Teamworks.Core
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public string Reply { get; set; }

        public static Message Forge(string text, string person)
        {
            return new Message()
                       {
                           Content = text,
                           Date = DateTime.Now,
                           Person = person,
                           Reply = null
                       };
        }
    }
}