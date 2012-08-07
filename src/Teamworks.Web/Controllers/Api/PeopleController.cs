using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    [DefaultHttpRouteConvention]
    public class PeopleController : RavenApiController
    {
    }
}