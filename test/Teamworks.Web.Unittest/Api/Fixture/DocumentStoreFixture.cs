using System.Security.Principal;
using System.Threading;
using Raven.Client.Embedded;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;

namespace Teamworks.Web.Uni.Api.Fixture
{
    public class DocumentStoreFixture
    {
        public void Initialize()
        {
            if (Global.Database == null)
            {
                Global.Database =
                new EmbeddableDocumentStore
                {
                     RunInMemory = true
                }.Initialize();
            }
            AutoMapperConfiguration.Configure();
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