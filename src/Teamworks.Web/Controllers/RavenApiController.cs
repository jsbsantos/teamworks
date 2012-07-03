using Raven.Client;
using System.Web.Http;
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