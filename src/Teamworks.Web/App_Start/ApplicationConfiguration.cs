using System;
using System.Reflection;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.App_Start
{
    public static class ApplicationConfiguration
    {
        public static void Register()
        {
            InitializeExecutor();
            InitializeAutoMapper();
            InitializeDocumentStore();
        }

        public static void InitializeExecutor()
        {
            Global.Executor = Executor.Instance;
            Global.Executor.Timeout = 15000;
            Global.Executor.Initialize();
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

            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), Global.Database);
        }
    }
}