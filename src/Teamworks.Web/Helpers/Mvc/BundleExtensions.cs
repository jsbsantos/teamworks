using System.Web.Optimization;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            var css = new StyleBundle("~/css")
                .Include("~/content/css/bootstrap/bootstrap.css",
                         "~/content/css/bootstrap/datepicker.css",
                         "~/content/css/font-awesome.css",
                         "~/content/css/teamworks.d3.css",
                         "~/content/css/teamworks.css",
                         "~/content/css/bootstrap/bootstrap-responsive.css");

            bundles.Add(css);

            var js = new ScriptBundle("~/js")
                // extra libs
                .Include("~/content/js/libs/date.js",
                         "~/content/js/libs/crypto/core.js",
                         "~/content/js/libs/crypto/md5.js")
                // bootstrap in the correct order
                .Include("~/content/js/libs/bootstrap/transaction.js",
                         "~/content/js/libs/bootstrap/alert.js",
                         "~/content/js/libs/bootstrap/button.js",
                         "~/content/js/libs/bootstrap/carousel.js",
                         "~/content/js/libs/bootstrap/collapse.js",
                         "~/content/js/libs/bootstrap/dropdown.js",
                         "~/content/js/libs/bootstrap/modal.js",
                         "~/content/js/libs/bootstrap/tooltip.js",
                         "~/content/js/libs/bootstrap/popover.js",
                         "~/content/js/libs/bootstrap/scrollspy.js",
                         "~/content/js/libs/bootstrap/tab.js",
                         "~/content/js/libs/bootstrap/typeahead.js",
                         "~/content/js/libs/bootstrap/datepicker.js")
                // d3
                .Include("~/content/js/libs/d3/d3.v2.js")
                // visual aids
                .Include("~/content/js/libs/knockout-2.0.0.js",
                         "~/content/js/libs/knockout.unobtrusive.js",
                         "~/content/js/libs/knockout.mapping.js",
                         "~/content/js/tw/tw.js",
                         "~/content/js/tw/tw.knockout.js")
                .IncludeDirectory("~/content/js/tw/", "*.pages.*")
                .Include("~/content/js/tw/tw.runner.js",
                         "~/content/js/tw/tw.d3.js");
#if !DEBUG            
            js.Include("~/content/js/gac.js");
#endif
            bundles.Add(js);
        }
    }
}