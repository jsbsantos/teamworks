using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Mvc
{
    public class DiscussionsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier, int projectid)
        {
            string model = string.Format("/api/projects/{0}/discussions/", projectid);
            return identifier != null
                       ? View("Discussion", (object) (model + identifier.Value))
                       : View("Discussions", (object) model);
        }
    }
}