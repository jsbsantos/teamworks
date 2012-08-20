using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Core.Services.Tasks
{
    public class SendNotificationsAsync
    {
        private IDocumentStore Store;
        private bool run = true;
        private AutoResetEvent ARE;

        private int tries = 1;
        private int baseTime = 60000;
        public int[] waitTimes = {1, 5, 10};

        private List<Discussion_Messages_PendingNotification.Result> Query(IDocumentSession DbSession)
        {
            return
                DbSession.Query
                    <Discussion_Messages_PendingNotification.Result, Discussion_Messages_PendingNotification>
                    ()
                    .Customize(
                        c => c.WaitForNonStaleResults())
                    .AsProjection<Discussion_Messages_PendingNotification.Result>().ToList();
        }

        public SendNotificationsAsync Initialize(IDocumentStore documentStore)
        {
            ARE = new AutoResetEvent(true);
            Store = documentStore;
            return this;
        }

        public void Start()
        {
            if (Store == null)
                throw new NullReferenceException("Document Store is null.");

            while (run)
            {
                List<Discussion_Messages_PendingNotification.Result> results;
                using (var session = Store.OpenSession())
                {
                    results = Query(session).ToList();
                }
                if (results.Count == 0)
                {
                    ARE.WaitOne(waitTimes[tries]*baseTime);
                    tries = tries == waitTimes.Length - 1 ? tries : tries++;
                }
                else
                {
                    tries = 1;

                    //do stuff
                }

            }
        }
    }
}