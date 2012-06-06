using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers.Web.Filters
{
    public class FormsAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var id = user.Identity.Name;
                if (!string.IsNullOrEmpty(id))
                {
                    var person = Global.Raven.CurrentSession.Load<Person>(id);
                    if (person != null)
                    {
                        context.HttpContext.User = new GenericPrincipal(new PersonIdentity(person), person.Roles.ToArray());
                    }
                }
            }
            base.OnActionExecuting(context);
        }
    }
}