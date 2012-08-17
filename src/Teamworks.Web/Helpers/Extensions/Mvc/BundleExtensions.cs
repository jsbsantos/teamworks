using System.Web.Optimization;

namespace Teamworks.Web.Helpers.Extensions.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            Bundle css = new StyleBundle("~/css")
                .Include("~/content/css/bootstrap/bootstrap.css",
                         "~/content/css/bootstrap/datepicker.css",
                         "~/content/css/font-awesome.css",
                         "~/content/css/teamworks.d3.css",
                         "~/content/css/teamworks.css",
                         "~/content/css/bootstrap/bootstrap-responsive.css");

            bundles.Add(css);

            Bundle js = new ScriptBundle("~/js")
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
                // knockout
                .IncludeDirectory("~/content/js/libs/", "knockout*")
                // tw
                .Include("~/content/js/tw/tw.js")
                .IncludeDirectory("~/content/js/tw/charts/", "tw.charts.*")
                .IncludeDirectory("~/content/js/tw/", "tw.pages.*")
                .Include("~/content/js/tw/tw.runner.js");
#if !DEBUG            
            js.Include("~/content/js/gac.js");
#endif
            bundles.Add(js);
        }
    }
}