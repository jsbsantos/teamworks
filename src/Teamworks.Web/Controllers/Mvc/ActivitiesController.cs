using System;
using System.Web.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class ActivitiesController : RavenDbController
    {
        
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int? identifier, int projectid)
        {
            string model = string.Format("/api/projects/{0}/activities/", projectid);
            return identifier != null
                       ? View("Activity", (object)(model + identifier.Value))
                       : View("Activities", (object) model);
        }
    }
}