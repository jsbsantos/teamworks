using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Mvc
{
    public class PeopleController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(DbSession.Query<Core.Person>().Select(
                        Mapper.Map<Core.Person, Person>));

            var person = DbSession.Load<Core.Person>("people/"+id);
            if (person == null)
            {
                throw new HttpException(404, "Not Found");
               
            }

            return View("Person", Mapper.Map<Core.Person, Person>(person));
        }
    }
}