using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api")]
    [DefaultHttpRouteConvention]
    public class PeopleController : RavenApiController
    {
        
    }
}