using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Teamworks.Web.Helpers
{
    public static class PageViewExtensions
    {
        public static HelperResult Identity(this WebViewPage _this)
        {
            return new HelperResult(writer => _this.Html.RenderPartial(_this.User.Identity.IsAuthenticated ? "Authenticated" : "Anonymous"));
        }
    }
}