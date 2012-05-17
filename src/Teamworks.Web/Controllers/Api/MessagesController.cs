using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects")]
    public class MessagesController : RavenApiController
    {
    }
}