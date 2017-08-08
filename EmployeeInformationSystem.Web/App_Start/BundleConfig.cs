using System.Web;
using System.Web.Optimization;

namespace EmployeeInformationSystem.Web
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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //bundles.Add(new Bundle("~/css/core").IncludeDirectory(@"~/Content/beagle_Theme/assets/lib/perfect-scrollbar/css", "perfect-scrollbar.css")
            //                                         .IncludeDirectory(@"~/Content/beagle_Theme/assets/lib/multiselect/css", "multi-select.css")
            //                                         .IncludeDirectory(@"~/Content/beagle_Theme/assets/css", "style.css"));


            bundles.Add(new StyleBundle("~/css/core").Include(
                  "~/Content/beagle_Theme/assets/lib/material-design-icons/css/material-design-iconic-font.css", new CssRewriteUrlTransform())
                  .Include("~/Content/beagle_Theme/assets/lib/perfect-scrollbar/css/perfect-scrollbar.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/datetimepicker/css/bootstrap-datetimepicker.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/daterangepicker/css/daterangepicker.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/select2/css/select2.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/bootstrap-slider/css/bootstrap-slider.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/jquery.magnific-popup/magnific-popup.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/bootstrap-multiselect/css/bootstrap-multiselect.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/lib/multiselect/css/multi-select.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/css/style.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/js/core").Include(
                    "~/Content/beagle_Theme/assets/lib/jquery/jquery.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Content/beagle_Theme/assets/js/main.js",
                    "~/Content/beagle_Theme/assets/lib/perfect-scrollbar/js/perfect-scrollbar.jquery.js",
                    "~/Content/beagle_Theme/assets/lib/bootstrap/dist/js/bootstrap.js",
                    "~/Content/beagle_Theme/assets/lib/jquery.nestable/jquery.nestable.js",
                    "~/Content/beagle_Theme/assets/lib/moment.js/min/moment.js",
                    "~/Content/beagle_Theme/assets/lib/datetimepicker/js/bootstrap-datetimepicker.js",
                    "~/Content/beagle_Theme/assets/lib/jquery-ui/jquery-ui.js",
                    "~/Content/beagle_Theme/assets/lib/daterangepicker/js/daterangepicker.js",
                    "~/Content/beagle_Theme/assets/lib/select2/js/select2.js",
                    "~/Content/beagle_Theme/assets/lib/select2/js/select2.full.js",
                    "~/Content/beagle_Theme/assets/lib/bootstrap-slider/js/bootstrap-slider.js",
                    "~/Content/beagle_Theme/assets/lib/jquery.magnific-popup/jquery.magnific-popup.js",
                    "~/Content/beagle_Theme/assets/lib/masonry/masonry.pkgd.js",
                    "~/Content/beagle_Theme/assets/lib/bootstrap-multiselect/js/bootstrap-multiselect.js",
                    "~/Content/beagle_Theme/assets/lib/multiselect/js/jquery.multi-select.js",
                    "~/Content/beagle_Theme/assets/lib/quicksearch/jquery.quicksearch.js"));

            bundles.Add(new StyleBundle("~/css/login").Include(
                  "~/Content/beagle_Theme/assets/lib/material-design-icons/css/material-design-iconic-font.css", new CssRewriteUrlTransform())
                  .Include("~/Content/beagle_Theme/assets/lib/perfect-scrollbar/css/perfect-scrollbar.css", new CssRewriteUrlTransform())
                   .Include("~/Content/beagle_Theme/assets/css/style.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/js/login").Include(
                    "~/Content/beagle_Theme/assets/lib/jquery/jquery.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Content/beagle_Theme/assets/lib/perfect-scrollbar/js/perfect-scrollbar.jquery.js",
                    "~/Content/beagle_Theme/assets/js/main.js",
                    "~/Content/beagle_Theme/assets/lib/bootstrap/dist/js/bootstrap.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
