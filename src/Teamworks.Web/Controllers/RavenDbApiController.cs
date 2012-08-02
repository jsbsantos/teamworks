using Raven.Client;
using System.Web.Http;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Controllers
{
    public class RavenDbApiController : ApiController
    {
        protected RavenDbApiController() { }

        protected RavenDbApiController(IDocumentSession session)
        {
            DbSession = session;
        }

        private IDocumentSession _session;
        protected IDocumentSession DbSession
        {
            set { _session = value; }
            get
            {
                return _session ?? (_session = Request.GetOrOpenCurrentSession());
            }
        }
    }
}