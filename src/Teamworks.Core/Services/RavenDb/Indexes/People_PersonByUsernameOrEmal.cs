using System.Linq;
using Raven.Client.Indexes;

namespace Teamworks.Core.Services.RavenDb.Indexes
{
    public class People_PersonByUsernameOrEmail : AbstractIndexCreationTask<Person>
    {

        public People_PersonByUsernameOrEmail()
        {
            Map = people => from person in people
                            select new {person.Username, person.Email};
        }
    }
}