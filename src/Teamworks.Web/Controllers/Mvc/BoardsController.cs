using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;
using Board = Teamworks.Web.Models.Board;
using Project = Teamworks.Core.Project;

namespace Teamworks.Web.Controllers.Mvc
{
    public class BoardsController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id, int projectid)
        {
            int discussionid;
            
            var project = DbSession
                .Include<Project>(p => p.Boards)
                .Load<Project>(projectid);

            if (project == null)
            {
                return new HttpNotFoundResult();
            }

            bool parse = int.TryParse(id, out discussionid);
            if (id != null && parse)
            {
                if (project.Boards.Count(t => t.Identifier() == discussionid) == 0)
                {
                    return new HttpNotFoundResult();
                }
                var topic = DbSession.Load<Core.Board>(discussionid);
                var model = Mapper.Map<Core.Board, Board>(topic);
                return View("Discussion", model);
            }

            return View(Mapper.Map<List<Core.Board>, List<DryBoard>>(DbSession.Load<Core.Board>(project.Boards).ToList()));
        }
    }
}