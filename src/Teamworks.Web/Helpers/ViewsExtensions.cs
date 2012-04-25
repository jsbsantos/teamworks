using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Teamworks.Web.Helpers
{
    public static class ViewsExtensions
    {
        public static HelperResult Identity(this HtmlHelper _this)
        {
            var user = HttpContext.Current.User;
            return
                new HelperResult(
                    writer =>
                    _this.RenderPartial(user.Identity.IsAuthenticated ? "Authenticated" : "Anonymous"));
        }

        public static bool IsDebugBuild(this HtmlHelper helper)
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}