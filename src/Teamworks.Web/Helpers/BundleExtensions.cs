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
#if DEBUG
            var css = new Bundle("~/css");
#else
            var css = new Bundle("~/css", new CssMinify());
#endif
            css.AddFile("~/content/css/bootstrap.css");
            css.AddFile("~/content/css/bootstrap-responsive.css");
            css.AddFile("~/content/css/teamworks.css");
            bundles.Add(css);

#if DEBUG
            var js = new Bundle("~/js");
#else
            var js = new Bundle("~/js", new JsMinify());
#endif
            js.AddFile("~/content/js/libs/knockout-2.0.0.js");
            js.AddFile("~/content/js/libs/knockout.unobtrusive.js");
#if !DEBUG
            js.AddFile("~/content/js/application.js");
            js.AddFile("~/content/js/application.bindings.js");
#endif
            bundles.Add(js);
        }
    }
}
