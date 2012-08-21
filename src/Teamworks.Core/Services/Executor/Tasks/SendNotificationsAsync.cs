using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core.Mailgun;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Core.Services.Executor.Tasks
{
    public class SendNotificationsAsync
    {
        private bool run = true;
        private int checkCount = 1;
        private const int BaseTimeout = 60000;
        private const int WaitFactor = 5;

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
                .Where(d => d.Messages.Any(m => !m.NotificationSent) && d.Subscribers.Count > 0)
                .ToList();

            return query.Select(d => new Notification
                {
                    Discussion = d.Id.ToIdentifier(),
                    Name = d.Name,
                    Messages = d.Messages.Where(m => !m.NotificationSent).ToList(),
                    Subscribers = dbSession.Load<Person>(d.Subscribers).Select(p => p.Email).ToList()
                }).ToList();
        }

        private int GetTimeout()
        {
            var result = WaitFactor*checkCount*BaseTimeout;
            checkCount = ++checkCount%25;
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
                        checkCount = 0;

                        foreach (var notification in results)
                        {
                            foreach (var message in notification.Messages)
                            {
                                SendNotificationEmail(notification.Discussion, notification.Name, message,
                                                      notification.Subscribers);
                            }
                        }
                    }

                    dbSession.SaveChanges();
                }

                Thread.Sleep(GetTimeout());
            }
        }

        private void SendNotificationEmail(int discussion, string name, Discussion.Message message,
                                           IEnumerable<string> people)
        {
            var receivers = string.Join(";", people);
            string id = String.Format("{0}.{1}.{2}@teamworks.mailgun.org",
                                      discussion, message.Id,
                                      DateTime.Now.ToString("yyyymmddhhMMss"));

            message.Reply = MailHub.Send(MailgunConfiguration.Host, receivers, name, message.Content, id);
            message.NotificationSent = true;
        }
    }
}