using System;
using Raven.Client;
using Raven.Client.Document;
using Teamworks.Core.Services.Storage;

namespace Teamworks.Core.Services.RavenDb
{
    public class Database
    {
        private const string Key = "RAVEN_CURRENT_SESSION_KEY";

        private static readonly Lazy<Database> _instance =
            new Lazy<Database>(() => new Database());

        public static IDocumentStore Store;

        public static Database Instance
        {
            get { return _instance.Value; }
        }
    }
}