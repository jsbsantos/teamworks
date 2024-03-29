using System;
using System.IO;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Teamworks.Core.Services;
using Teamworks.Core.Services.Async;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.App_Start
{
    public static class ApplicationConfiguration
    {
        public static void Register()
        {
            InitializeAutoMapper();
            InitializeDocumentStore();
            InitializeMessageNotificationsSender();
        }

        private static void InitializeMessageNotificationsSender()
        {
            var sender = new SendNotificationsAsync();
            Task.Factory.StartNew(sender.Run).ContinueWith(task =>
                {
                    return;
                });
        }

        public static void InitializeAutoMapper()
        {
            AutoMapperConfiguration.Configure();
        }

        public static void InitializeDocumentStore()
        {
            try
            {
                new Uri("http://fail/first/time?only=%2bplus");
            }
            catch (Exception)
            {
            }

            if (Global.Database != null) return; // prevent misuse

            Global.Database =
                new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    }.Initialize();
            IndexCreation.CreateIndexes(typeof(ActivitiesDuration).Assembly, Global.Database);
        }
    }
}