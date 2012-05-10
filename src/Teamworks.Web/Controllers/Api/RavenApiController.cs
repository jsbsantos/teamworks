using System.Web.Http;
using Raven.Client;
using Teamworks.Core.Extensions;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Controllers.Api {
    public class RavenApiController : ApiController {
        public IDocumentSession DbSession {
            get { return Local.Data[Core.Extensions.Global.RavenKey] as IDocumentSession; }
        }
    }
}