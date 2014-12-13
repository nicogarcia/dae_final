using System.Collections.Generic;
using System.IO;
using System.Web.Optimization;

namespace Presentacion
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.datetimepicker.js",
                        "~/Scripts/selectize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/fullcalendar.min.js",
                        "~/Scripts/fullcalendar.lang.es.js",
                        "~/Scripts/calendar.config.js"));

            var validationBundle = new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/globalize/globalize.js",
                "~/Scripts/globalize/cultures/globalize.culture.es-AR.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.validate.globalize.min.js");
            validationBundle.Orderer = new NonOrderingBundleOrderer();
            
            bundles.Add(validationBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                        "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/selectize.bootstrap3.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/all.css",
                        "~/Content/jquery.datetimepicker.css",
                        "~/Content/fullcalendar.min.css"));
        }
    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}