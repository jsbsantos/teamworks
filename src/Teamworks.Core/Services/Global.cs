using Raven.Client;

namespace Teamworks.Core.Services
{
    public static class Global
    {

        public static IDocumentStore Database;

        public static class Constants
        {
            public const string Operation = "GOD";
        }
    }
}