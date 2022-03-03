using System.Web;
using System.Web.Optimization;

namespace WebTurismo
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/assets/css/plugins.css",
                      "~/Content/assets/css/nav.css",
                      "~/Content/assets/css/style-starter.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/Web").Include(

                        "~/Content/assets/css/style-starter.css",
                        "~/Content/alertifyjs/css/alertify.css",
                        "~/Content/alertifyjs/css/themes/default.css",
                        "~/Content/JqueryUI/jquery-ui.css",
                        "~/Content/JqueryUI/jquery-ui.min.css",
                        "~/Content/JqueryUI/jquery-ui.structure.css",
                        "~/Content/JqueryUI/jquery-ui.structure.min.css",
                        "~/Content/JqueryUI/jquery-ui.theme.css",
                        "~/Content/JqueryUI/jquery-ui.theme.min.css"
                        //"~/Content/alertifyjs/css/alertify.rtl.css",
                        //"~/Content/alertifyjs/css/alertify.rtl.min.css"



                        ));

            bundles.Add(new ScriptBundle("~/bundles/web").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Content/assets/js/jquery-{version}.js",
                "~/Content/assets/js/jquery.waypoints.min.js",
                "~/Content/assets/js/jquery.countup.js",
                "~/Content/assets/js/owl.carousel.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Scripts/WebTurismo/Main.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/jquery.magnific-popup.min.js",
                 "~/Content/alertifyjs/alertify.js",
                "~/Scripts/WebTurismo/Login.js",
                "~/Scripts/WebTurismo/Main.js",
                "~/Scripts/WebTurismo/Registrar.js",
                "~/Scripts/WebTurismo/Mensajeria.js",
                "~/Scripts/WebTurismo/Logout.js",
                "~/Scripts/WebTurismo/Ubicacion.js",
                "~/Scripts/WebTurismo/BuscarPropiedad.js",
                "~/Scripts/WebTurismo/Pago.js",
                "~/Scripts/JqueryUI/jquery-ui.js",
                "~/Scripts/moment.js"

                ));
                      
        }
    }
}
