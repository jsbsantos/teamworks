using Raven.Client;
using System.Web.Http;

namespace Teamworks.Web.Controllers
{
    public abstract class RavenApiController : ApiController
    {
        protected RavenApiController()
        {
           
        }

        protected RavenApiController(IDocumentSession session)
        {
            DbSession = session;
        }

        public IDocumentSession DbSession { set; get; }
    }
}