using System.Web.Optimization;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            var css = new StyleBundle("~/css").Include("~/content/css/bootstrap/bootstrap.css", "~/content/css/font-awesome.css",
                        "~/content/css/teamworks.css", "~/content/css/bootstrap/bootstrap-responsive.css");

            bundles.Add(css);

            var js = new ScriptBundle("~/js").Include("~/content/js/libs/iso8601.js");
#if !DEBUG
            js.Include("~/content/js/libs/bootstrap/bootstrap.js", "~/content/js/libs/knockout-2.0.0.js",
                       "~/content/js/libs/knockout.unobtrusive.js", "~/content/js/app/teamworks.js",
                       "~/content/js/app/teamworks.viewmodels.models.js", "~/content/js/app/teamworks.viewmodels.js",
                       "~/content/js/app/teamworks.start.js","~/content/js/gac.js");
#endif
            
            bundles.Add(js);
        }
    }
}