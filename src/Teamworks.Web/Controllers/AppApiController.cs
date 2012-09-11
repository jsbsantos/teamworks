using System.Web.Http;
using Raven.Client;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public abstract class AppApiController : ApiController
    {
        public IDocumentSession DbSession { set; get; }
    }
}