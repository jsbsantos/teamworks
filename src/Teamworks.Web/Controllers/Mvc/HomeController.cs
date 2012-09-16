using System.Linq;
using System.Web.Mvc;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Web.Helpers.Extensions.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    public class HomeController : AppController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var current = DbSession.GetCurrentPerson();

                var activities = DbSession.Query<Activity>()
                    .Include(a => a.Project)
                    .Where(a => a.People.Any(p => p == current.Id)).ToList();
                
                return View(activities);
            }
            return View("Welcome");
        }

        public ActionResult Welcome()
        {
           
            return View();
        }
    }
}