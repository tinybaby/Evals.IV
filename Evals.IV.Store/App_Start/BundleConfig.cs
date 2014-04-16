using System.Web;
using System.Web.Optimization;

namespace Evals.IV.Store
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/areas/admin").Include(
                "~/Areas/Administration/Content/style.css"
                ));

            bundles.Add(new StyleBundle("~/Assets/css").Include(
                "~/Assets/bootstrap/css/bootstrap.min.css",
                "~/Assets/icons/flags/flags.css",
                "~/Assets/icons/font-awesome/css/font-awesome.min.css",
                "~/Assets/icons/ionicons/css/ionicons.min.css",
                "~/Assets/css/style.css",
                "~/Assets/lib/novus-nvd3/nv.d3.min.css",
                "~/Assets/lib/owl-carousel/owl.carousel.css",
                "~/Assets/css/video.css"
                ));
            bundles.Add(new ScriptBundle("~/Assets/moment").Include("~/assets/lib/moment-js/moment.min.js"));

            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                "~/Assets/js/jquery.easing.1.3.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Assets/js/tinynav.js",
                "~/Assets/lib/perfect-scrollbar/min/perfect.scrollbar.min.js",
                "~/Assets/js/tisa_common.js",
                "~/Assets/js/tisa_style_switcher.js",
                "~/Assets/lib/d3/d3.min.js",
                "~/Assets/lib/novus-nvd3/nv.d3.min.js",
                "~/Assets/lib/flot/jquery.flot.min.js",
                "~/Assets/lib/flot/jquery.flot.pie.min.js",
                "~/Assets/lib/flot/jquery.flot.resize.min.js",
                "~/Assets/lib/flot/jquery.flot.tooltip.min.js",
                "~/Assets/lib/underscore-js/underscore-min.js",
                "~/Assets/lib/CLNDR/src/clndr.js",
                "~/Assets/lib/easy-pie-chart/dist/jquery.easypiechart.min.js",
                "~/Assets/lib/owl-carousel/owl.carousel.min.js",
                "~/Assets/js/apps/tisa_dashboard.js"
                ));
        }
    }
}



