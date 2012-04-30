using System.Web.Mvc;
using Raven.Client;
using Teamworks.Core.Extensions;

namespace Teamworks.Web.Controllers
{
    [RavenInitializerFilter]
    public class RavenController : Controller
    {
        public IDocumentSession RavenSession { get { return Local.Data[Global.RavenSessionkey] as IDocumentSession; } }
    }
}