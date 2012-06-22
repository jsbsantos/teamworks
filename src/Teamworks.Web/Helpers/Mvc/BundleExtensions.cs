using Microsoft.Web.Optimization;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            var css = new Bundle("~/css", typeof (CssMinify));
            css.AddFile("~/content/css/bootstrap/bootstrap.css");
            css.AddFile("~/content/css/font-awesome.css");
            css.AddFile("~/content/css/teamworks.css");
            css.AddFile("~/content/css/bootstrap/bootstrap-responsive.css");
            
            bundles.Add(css);

            var js = new Bundle("~/js", typeof (JsMinify));
#if !DEBUG
            js.AddFile("~/content/js/libs/bootstrap/bootstrap.js");
            js.AddFile("~/content/js/libs/knockout-2.0.0.js");
            js.AddFile("~/content/js/libs/knockout.unobtrusive.js");

            js.AddFile("~/content/js/app/teamworks.app.js");
            js.AddFile("~/content/js/app/teamworks.viewmodels.js");
            js.AddFile("~/content/js/app/teamworks.pages.js");
            js.AddFile("~/content/js/app/teamworks.js");
            js.AddFile("~/content/js/gac.js");
#endif
            bundles.Add(js);
        }
    }
}