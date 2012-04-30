using System.Web.Mvc;
using Teamworks.Core.Extensions;

namespace Teamworks.Web.Controllers
{
    public class RavenInitializerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenController controller = filterContext.Controller as RavenController;
            if (controller != null && controller.RavenSession == null)
            {
                Local.Data[Global.RavenSessionkey] = RavenSessionManager.NewSession();
            }
        }
    }
}