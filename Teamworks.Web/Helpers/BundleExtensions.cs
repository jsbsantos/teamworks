using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Optimization;

namespace Teamworks.Web.Helpers
{
    public static class BundleExtensions
    {
        public class AsIsBundleOrderer : IBundleOrderer
        {
            public virtual IEnumerable<FileInfo> OrderFiles(BundleContext context, IEnumerable<FileInfo> files)
            {
                if (context == null)
                    throw new ArgumentNullException("context");

                if (files == null)
                    throw new ArgumentNullException("files");
                return files;
            }
        }

        public static void EnableTeamworksBundle(this BundleCollection bundles)
        {
            // todo var css = new Bundle("~/css", new CssMinify());
            var css = new Bundle("~/css");
            css.AddFile("~/content/css/bootstrap.css");
            css.AddFile("~/content/css/bootstrap-responsive.css");
            css.AddFile("~/content/css/teamworks.css");
            bundles.Add(css);

            // todo var js = new Bundle("~/js", new JsMinify());
            var js = new Bundle("~/js");
            js.AddFile("~/content/js/libs/knockout-2.0.0.js");
            js.AddFile("~/content/js/libs/tabs.js");
            bundles.Add(js);
        }
    }
}
