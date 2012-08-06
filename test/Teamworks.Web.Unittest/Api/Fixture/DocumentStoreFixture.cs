using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Raven.Client.Embedded;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.Uni.Api.Fixture
{
    public class DocumentStoreFixture
    {
        public void Initialize()
        {
            if (Global.Database != null) return;
            Global.Database =
                new EmbeddableDocumentStore
                    {
                        RunInMemory = true
                    }.Initialize();
            AutoMapperConfiguration.Configure();
            PopulateDatabase();
        }

        private void PopulateDatabase()
        {
            using (var session = Global.Database.OpenSession())
            {
                var person = Person.Forge("email@email.pt", "username", "password", "Person");
                session.Store(person);
                foreach (var p in Enumerable.Range(1, 3))
                {
                    var project = Project.Forge("Proj " + p, "Desc " + p);
                    session.Store(project);
                    foreach (var e in Enumerable.Range(1,3))
                    {
                        var activity = Activity.Forge(project.Id, "Act " + e, "Desc " + e, 20*e);
                        var discussion = Discussion.Forge("Disc " + e, "Content " + e, project.Id, person.Id);
                        session.Store(activity);
                        session.Store(discussion);
                    }
                }
                session.SaveChanges();
            }
        }

        public void Store(object entity)
        {
            using (var session = Global.Database.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }
        }

        public T Load<T>(string id)
        {
            using (var session = Global.Database.OpenSession())
            {
                return session.Load<T>(id);
            }
        }

        public void InjectPersonAsCurrentIdentity(Person person)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new PersonIdentity(person), new string[0]);
        }
    }
}