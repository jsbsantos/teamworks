using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client.Linq;
using Teamworks.Core.Services.RavenDb.Indexes;

namespace Teamworks.Web.Controllers.Mvc
{
    public class HomeController : RavenController
    {
        [HttpGet]
        [AllowAnonymous]
        [ActionName("View")]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            return View("Welcome");
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Test()
        {
            IRavenQueryable<ProjectEntityCount.Result> content = DbSession
                .Query<ProjectEntityCount.Result, ProjectEntityCount>()
                .Where(r => r.Project == "projects/1");


            return new ContentResult
                       {
                           Content = JsonConvert.SerializeObject(content,
                                                                 new JsonSerializerSettings
                                                                     {
                                                                         ContractResolver =
                                                                             new CamelCasePropertyNamesContractResolver(),
                                                                         NullValueHandling = NullValueHandling.Ignore
                                                                     })
                       };
        }
    }
}