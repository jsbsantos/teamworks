using System.Web.Optimization;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            Bundle css = new StyleBundle("~/content/css").Include("~/content/css/bootstrap/bootstrap.css",
                                                          "~/content/css/font-awesome.css",
                                                          "~/content/css/teamworks.css",
                                                          "~/content/css/bootstrap/bootstrap-responsive.css");

            bundles.Add(css);

            Bundle js = new ScriptBundle("~/content/js").Include("~/content/js/libs/iso8601.js",
                "~/content/js/libs/crypto/core.js",
                                                         "~/content/js/libs/crypto/md5.js",
                                                         "~/content/js/libs/bootstrap/bootstrap.js",
                                                         "~/content/js/libs/knockout-2.0.0.js",
                                                         "~/content/js/libs/knockout.unobtrusive.js",
                                                         "~/content/js/app/teamworks.js",
                                                         "~/content/js/app/teamworks.viewmodels.models.js",
                                                         "~/content/js/app/teamworks.viewmodels.js",
                                                         "~/content/js/app/teamworks.start.js");
#if !DEBUG            
            js.Include("~/content/js/gac.js");
#endif
            bundles.Add(js);
        }
    }
}