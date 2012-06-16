using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Controllers.Web
{
    public class DiscussionsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id, int projectid)
        {
            int discussionid;
            
            var project = DbSession
                .Include<Project>(p => p.Threads)
                .Load<Project>(projectid);

            if (project == null)
            {
                return new HttpNotFoundResult();
            }

            bool parse = int.TryParse(id, out discussionid);
            if (id != null && parse)
            {
                if (project.Threads.Count(t => t.Identifier() == discussionid) == 0)
                {
                    return new HttpNotFoundResult();
                }
                var topic = DbSession.Load<Thread>(discussionid);
                var model = Mapper.Map<Thread, ThreadModel>(topic);
                return View("Discussion", model);
            }

            return View(Mapper.Map<List<Thread>, List<DryThreadModel>>(DbSession.Load<Thread>(project.Threads).ToList()));
        }
    }
}