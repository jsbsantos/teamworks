using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Teamworks.Core;
using Teamworks.Core.Mailgun;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class DiscussionExtensions
    {
        public static void Notify(this Discussion thread, Message message)
        {
            var emails = Global.Database.CurrentSession.Load<Person>(thread.Subscribers)
                .Select(x => x.Email).ToList();
            
            if (emails.Count > 0)
            {
                var notifications = new StringBuilder();
                foreach (var email in emails)
                {
                    notifications.Append(email);
                    notifications.Append(";");
                }

                var id = String.Format("{0}.{1}.{2}@teamworks.mailgun.org",
                                       thread.Identifier, message.Id, DateTime.Now.ToString("yyyymmddhhMMss"));

                message.Reply = MailHub.Send(MailgunConfiguration.Host,
                                             notifications.ToString().TrimEnd(new[] {';'}),
                                             thread.Name,
                                             message.Content,
                                             id);
            }
        }
    }
}