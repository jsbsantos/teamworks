using System.Net;
using System.Web.Http;
using Raven.Client;

using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers
{
    public class RavenApiController : ApiController
    {
        protected IDocumentSession DbSession
        {
            get { return Global.Database.CurrentSession; }
        }
    }
}