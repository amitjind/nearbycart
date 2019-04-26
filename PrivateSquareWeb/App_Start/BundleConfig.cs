using System.Web;
using System.Web.Optimization;

namespace PrivateSquareWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/theme/lib/jquery/js/jquery.js",
                        "~/theme/lib/popper.js/js/popper.js",
                        "~/theme/lib/bootstrap/js/bootstrap.js",
                        "~/theme/lib/jquery.cookie/js/jquery.cookie.js",
                        "~/theme/lib/select2/js/select2.full.min.js",
                        "~/theme/lib/jquery-ui/js/jquery-ui.js",
                        "~/theme/js/slim.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/theme/lib/font-awesome/css/font-awesome.css",
                      "~/theme/lib/Ionicons/css/ionicons.css",
                      "~/theme/lib/select2/css/select2.min.css",
                      "~/theme/css/slim.css"));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/websitetheme/plugins/revolution/css").Include(
                                    "~/WebSiteTheme/plugins/font-awesome/css/font-awesome.min.css",
                                    "~/WebSiteTheme/plugins/ps-icon/style.css",
                                    "~/WebSiteTheme/plugins/bootstrap/dist/css/bootstrap.min.css",
                                    "~/WebSiteTheme/plugins/owl-carousel/assets/owl.carousel.css",
                                    "~/WebSiteTheme/plugins/bootstrap-select/dist/css/bootstrap-select.min.css",
                                    "~/WebSiteTheme/plugins/jquery-bar-rating/dist/themes/fontawesome-stars.css",
                                    "~/WebSiteTheme/plugins/slick/slick/slick.css",
                                    "~/WebSiteTheme/css/style.css",
                                    "~/WebSiteTheme/plugins/revolution/css/settings.css",
                                    "~/WebSiteTheme/plugins/revolution/css/layers.css",
                                    "~/WebSiteTheme/plugins/revolution/css/navigation.css"
                                ));

            bundles.Add(new ScriptBundle("~/websitetheme/bundles/jquery").Include(
                                                                "~/WebSiteTheme/plugins/jquery/dist/jquery.min.js",
                                                                "~/WebSiteTheme/plugins/bootstrap/dist/js/bootstrap.min.js",

                                                                "~/WebSiteTheme/plugins/owl-carousel/owl.carousel.min.js",
                                                                "~/WebSiteTheme/plugins/bootstrap-select/dist/js/bootstrap-select.min.js",

                                                                "~/WebSiteTheme/plugins/jquery-bar-rating/dist/jquery.barrating.min.js",
                                                                "~/WebSiteTheme/plugins/imagesloaded.pkgd.js",

                                                                "~/WebSiteTheme/plugins/masonry.pkgd.min.js",
                                                                "~/WebSiteTheme/plugins/isotope.pkgd.min.js",

                                                                "~/WebSiteTheme/plugins/slick/slick/slick.min.js",
                                                                "~/WebSiteTheme/plugins/jquery.matchHeight-min.js",

                                                                "~/WebSiteTheme/plugins/elevatezoom/jquery.elevatezoom.js",
                                                                "~/WebSiteTheme/plugins/gmap3.min.js",

                                                                "~/WebSiteTheme/plugins/revolution/js/jquery.themepunch.tools.min.js",
                                                                "~/WebSiteTheme/plugins/revolution/js/jquery.themepunch.revolution.min.js",

                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.video.min.js",
                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.slideanims.min.js",

                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.layeranimation.min.js",
                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.navigation.min.js",

                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.parallax.min.js",
                                                                "~/WebSiteTheme/plugins/revolution/js/extensions/revolution.extension.actions.min.js",
                                                                "~/WebSiteTheme/js/slider-1.js",
                                                                "~/WebSiteTheme/js/main.js"

                                                            ));

            BundleTable.EnableOptimizations = false;
        }
    }
}

