using System.Web;
using System.Web.Optimization;

namespace PoolHockeyMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/views/news.js"));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/home.css",
                      "~/Content/views/news.css"));

            // Theme JS
            bundles.Add(new ScriptBundle("~/theme/script").Include(
                      "~/theme/js/jquery.easing.min.js",
                      "~/theme/js/classie.js",
                      "~/theme/js/gnmenu.js",
                      "~/theme/js/jquery.scrollTo.js",
                      "~/theme/js/nivo-lightbox.min.js",
                      "~/theme/js/stellar.js",
                      "~/theme/js/custom.js"
                      ));

            // Theme CSS
            bundles.Add(new StyleBundle("~/theme/styles").Include(
                      "~/theme/css/nivo-lightbox.css",
                      "~/theme/css/nivo-lightbox-theme/default/default.css",
                      "~/theme/css/animate.css",
                      "~/theme/css/style.css",
                      "~/theme/color/default.css",
                      "~/theme/fonts/ecoicons/ecoicons.woff"
                      ));


        }
    }
}
