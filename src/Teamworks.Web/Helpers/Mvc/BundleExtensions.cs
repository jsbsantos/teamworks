using System.Web.Optimization;

namespace Teamworks.Web.Helpers.Mvc
{
    public static class BundleExtensions
    {
        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            Bundle css = new StyleBundle("~/css")
                .Include("~/content/css/bootstrap/bootstrap.css",
                         "~/content/css/bootstrap/datepicker.css",
                         "~/content/css/font-awesome.css",
                         "~/content/css/teamworks.css",
                         "~/content/css/bootstrap/bootstrap-responsive.css");

            bundles.Add(css);

            Bundle js = new ScriptBundle("~/js")
                .Include("~/content/js/libs/date.js",
                         "~/content/js/libs/crypto/core.js",
                         "~/content/js/libs/crypto/md5.js")
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
                .Include("~/content/js/libs/d3js/d3.v2.js")
                .Include("~/content/js/libs/knockout-2.0.0.js",
                         "~/content/js/libs/knockout.unobtrusive.js",
                         "~/content/js/app/tw.base.js",
                         "~/content/js/app/tw.knockout.js",
                         "~/content/js/app/tw.d3.js",
                         "~/content/js/app/tw.viewmodels.models.js",
                         "~/content/js/app/tw.viewmodels.js");
#if !DEBUG            
            js.Include("~/content/js/gac.js");
#endif
            bundles.Add(js);
        }
    }
}