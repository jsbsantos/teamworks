using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/events")]
    public class EventsController : RavenApiController
    {
    }
}