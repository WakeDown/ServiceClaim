using System.Web;
using System.Web.Optimization;

namespace ServiceClaim
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/jquery-ui.js",
                       "~/Scripts/jquery.unobtrusive-ajax.js",
                       "~/Scripts/jquery.mask.min.js",
                       "~/Scripts/jquery-dateformat.js"
                       //"~/Scripts/jquery-autocomplete.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrapValidator.js",
                      "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datepicker.ru.js",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/bootstrap-dialog.js",
                      //"~/Scripts/bootstrap-timepicker.js",
                      "~/Scripts/validator.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/site.js"));

            //bundles.Add(new ScriptBundle("~/bundles/common").Include(
            //            "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrapValidator.css",
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-timepicker.min.css",
                      "~/Content/bootstrap-multiselect.min.css",
                      "~/Content/bootstrap-dialog.css",
                       //"~/Content/jquery-autocomplete.css",
                      "~/Content/site.css",
                      "~/Content/animation.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-timepicker.min.css",
                      "~/Content/bootstrap-multiselect.min.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}
