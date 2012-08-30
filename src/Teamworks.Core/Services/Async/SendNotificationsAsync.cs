using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core.Mailgun;

namespace Teamworks.Core.Services.Async
{
    public class SendNotificationsAsync
    {
        private bool run = true;
        private int checkCount = 1;
        private const int BaseTimeout = 60000;
        private const int WaitFactor = 1;

        private class Notification
        {
            public int Discussion { get; set; }
            public string Name { get; set; }
            public List<Discussion.Message> Messages { get; set; }
            public List<string> Subscribers { get; set; }
        }

        private List<Notification> Query(IDocumentSession dbSession)
        {
            var query = dbSession.Query<Discussion>()
                .Customize(c => c.Include<Discussion>(d => d.Subscribers))
                .Where(d => d.Messages.Any(m => !m.NotificationSent))
                .ToList();

            return query.Select(d => new Notification
                {
                    Discussion = d.Id.ToIdentifier(),
                    Name = d.Name,
                    Messages = d.Messages.Where(m => !m.NotificationSent).ToList(),
                    Subscribers =
                        d.Subscribers.Count == 0
                            ? new List<string>()
                            : dbSession.Load<Person>(d.Subscribers).Select(p => p.Email).ToList()
                }).ToList();
        }

        private int GetTimeout()
        {
            var result = WaitFactor*checkCount*BaseTimeout;
            checkCount = checkCount == 10 ? 10 : ++checkCount;
            return result;
        }

        public void Run()
        {
            if (Global.Database == null)
                throw new NullReferenceException("Document Store is null.");
            while (run)
            {
                using (var dbSession = Global.Database.OpenSession())
                {
                    var results = Query(dbSession);

                    if (results.Count > 0)
                    {
                        bool success = true;
                        foreach (var notification in results)
                        {
                            foreach (var message in notification.Messages)
                            {
                                success &= SendNotificationEmail(notification.Discussion, notification.Name, message,
                                                                 notification.Subscribers);
                            }
                        }
                        if (success)
                            checkCount = 1;

                        dbSession.SaveChanges();
                    }
                }

                Thread.Sleep(GetTimeout());
            }
        }

        private bool SendNotificationEmail(int discussion, string name, Discussion.Message message,
                                           IEnumerable<string> people)
        {
            if (people.Any())
            {
                var receivers = string.Join(";", people);
                string id = String.Format("{0}.{1}.{2}@teamworks.mailgun.org",
                                          discussion, message.Id,
                                          DateTime.Now.ToString("yyyymmddhhMMss"));

                try
                {
                    message.Reply = MailHub.Send(MailgunConfiguration.Host, receivers, name, message.Content, id);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return (message.NotificationSent = true);
        }
    }
}