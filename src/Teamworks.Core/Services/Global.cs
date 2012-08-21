using Raven.Client;

namespace Teamworks.Core.Services
{
    public static class Global
    {
        public static IDocumentStore Database;
        public static Executor.Executor Executor { get; set; }

        #region Nested type: Constants

        public static class Constants
        {
            public const string Projects = "projects";
        }

        #endregion
    }
}