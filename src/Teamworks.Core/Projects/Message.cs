using System;

namespace Teamworks.Core.Projects
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }

        public static Message Forge(string text, string person)
        {
            return new Message()
                       {
                           Text = text,
                           Date = DateTime.Now,
                           Person = person
                       };
        }
    }
}