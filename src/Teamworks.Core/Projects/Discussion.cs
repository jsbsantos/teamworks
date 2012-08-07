using System;
using System.Collections.Generic;
using System.Text;
using Teamworks.Core.Mailgun;

namespace Teamworks.Core
{
    public class Discussion : Entity
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
        public string Entity { get; set; }
        public IList<Message> Messages { get; set; }
        public IList<string> Subscribers { get; set; }

        public int LastMessageId { get; set; }
        public string Name { get; set; }

        public int GenerateNewTimeEntryId()
        {
            return ++LastMessageId;
        }

        public static Discussion Forge(string name, string content, string entity, string person)
        {
            return new Discussion
                       {
                           Name = name,
                           Content = content ?? "",
                           Date = DateTime.Now,
                           Messages = new List<Message>(),
                           Subscribers = new List<string> {person},
                           Person = person,
                           LastMessageId = 0,
                           Entity = entity
                       };
        }

        public void Notify(Message message, IList<string> emails)
        {
            if (emails.Count <= 0) return;
            var notifications = new StringBuilder();
            foreach (string email in emails)
            {
                notifications.Append(email);
                notifications.Append(";");
            }

            string id = String.Format("{0}.{1}.{2}@teamworks.mailgun.org",
                                      Identifier, message.Id, DateTime.Now.ToString("yyyymmddhhMMss"));

            message.Reply = MailHub.Send(MailgunConfiguration.Host,
                                         notifications.ToString().TrimEnd(new[] {';'}),
                                         Name,
                                         message.Content,
                                         id);
        }
    }
}