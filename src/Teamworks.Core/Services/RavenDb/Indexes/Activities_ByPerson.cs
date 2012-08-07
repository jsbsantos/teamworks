using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class Activities_ByPerson : AbstractIndexCreationTask<Activity, Activities_ByPerson.Result>
    {
        public Activities_ByPerson()
        {
            Map = activities => from act in activities
                                from person in act.People
                                select new
                                           {
                                               Person = person
                                           };
        }

        #region Nested type: Result

        public class Result
        {
            public string Person { get; set; }
        }

        #endregion
    }
}