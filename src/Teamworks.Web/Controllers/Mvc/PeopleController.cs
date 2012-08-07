using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core;

namespace Teamworks.Web.Controllers.Mvc
{
    public class PeopleController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(DbSession.Query<Person>().Select(
                    Mapper.Map<Person, Models.Api.Person>));

            var person = DbSession.Load<Person>("people/" + id);
            if (person == null)
            {
                throw new HttpException(404, "Not Found");
            }

            return View("Person", Mapper.Map<Person, Models.Api.Person>(person));
        }
    }
}