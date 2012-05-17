using Microsoft.Web.Optimization;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            var css = new Bundle("~/css", typeof (CssMinify));
            css.AddFile("~/content/css/bootstrap/bootstrap.css");
            css.AddFile("~/content/css/bootstrap/bootstrap-responsive.css");
            css.AddFile("~/content/css/font-awesome.css");
            css.AddFile("~/content/css/teamworks.css");

            bundles.Add(css);

            var js = new Bundle("~/js", typeof (JsMinify));
#if !DEBUG
            js.AddFile("~/content/js/libs/knockout-2.0.0.js");
            js.AddFile("~/content/js/libs/knockout.unobtrusive.js");
            js.AddFile("~/content/js/application.viewmodels.js");
            js.AddFile("~/content/js/application.js");
#endif
            bundles.Add(js);
        }
    }
}