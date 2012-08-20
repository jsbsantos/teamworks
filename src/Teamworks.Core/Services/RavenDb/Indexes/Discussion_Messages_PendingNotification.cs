using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class Discussion_Messages_PendingNotification :
        AbstractIndexCreationTask<Discussion,Discussion_Messages_PendingNotification.Result>
    {
        public Discussion_Messages_PendingNotification()
        {
            Map = discussions => from disc in discussions
                                 from sub in disc.Subscribers
                                 from message in disc.Messages
                                 select new
                                     {
                                         Message = message.Id,
                                         Discussion = disc.Id,
                                         Content = message.Content,
                                         NotificationSent = message.NotificationSent,
                                         Person = sub,
                                         Email = (string) null
                                     };

            TransformResults = (database, results) => 
                                    from result in results
                                select new
                                    {
                                        Message = result.Message,
                                        Discussion = result.Discussion,
                                        Content = result.Content,
                                        Person = result.Person,
                                        Email = database.Load<Person>(result.Person).Email,
                                        NotificationSent = result.NotificationSent
                                    };

            Store(r => r.Message, FieldStorage.Yes);
            Store(r => r.Discussion, FieldStorage.Yes);
            Store(r => r.Content, FieldStorage.Yes);
            Store(r => r.NotificationSent, FieldStorage.Yes);

            Index(x => x.Message, FieldIndexing.NotAnalyzed);
            Index(x => x.Discussion, FieldIndexing.NotAnalyzed);
            Index(x => x.Person, FieldIndexing.NotAnalyzed);

        }

        #region Nested type: Result

        public class Result
        {
            public string Message { get; set; }
            public string Discussion { get; set; }
            public string Content { get; set; }
            public string Person { get; set; }
            public string Email { get; set; }
            public bool NotificationSent { get; set; }
        }

        #endregion
    }
}