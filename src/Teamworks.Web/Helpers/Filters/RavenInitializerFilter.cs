using System.Web.Mvc;
using Teamworks.Core.Extensions;
using Teamworks.Web.Controllers;
using Global = Teamworks.Web.Models.Global;

namespace Teamworks.Web.Helpers.Filters {
    public class RavenInitializerFilter : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var controller = filterContext.Controller as RavenController;
            if (controller != null && controller.DbSession == null) {
                Local.Data[Global.RavenKey] = RavenSessionManager.NewSession();
            }
        }
    }
}