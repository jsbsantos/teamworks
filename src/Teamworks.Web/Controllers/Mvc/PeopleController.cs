using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Controllers.Web
{
    public class PeopleController : RavenController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(DbSession.Query<Person>().Select(
                        Mapper.Map<Person, PersonModel>));

            var person = DbSession.Load<Person>("people/"+id);
            if (person == null)
            {
                throw new HttpException(404, "Not Found");
               
            }

            return View("Person", Mapper.Map<Person, PersonModel>(person));
        }
    }
}