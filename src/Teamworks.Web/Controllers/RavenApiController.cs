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
            get { return Global.Raven.CurrentSession; }
        }

        protected T Get<T>(int id) where T : class
        {
            var o = DbSession.Load<T>(id);
            if (o == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return o;
        }

        protected T Get<T>(string id) where T : class
        {
            var o = DbSession.Load<T>(id);
            if (o == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return o;
        }
    }
}