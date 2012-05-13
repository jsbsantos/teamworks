using System.Web.Http;
using Raven.Client;
using Teamworks.Core;

namespace Teamworks.Web.Controllers.Base {
    [Authorize]
    public class RavenApiController : ApiController {
        public IDocumentSession DbSession {
            get { return Global.Raven.CurrentSession; }
        }
    }
}