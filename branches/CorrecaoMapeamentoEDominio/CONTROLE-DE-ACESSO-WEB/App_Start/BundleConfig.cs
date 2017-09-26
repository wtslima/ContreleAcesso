using System.Web.Optimization;

namespace CONTROLE_DE_ACESSO_WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/jsScripts").Include(
                       "~/Scripts/jquery-1.10.2.min.js",
                       "~/Scripts/bootstrap.min.js",
                       "~/Scripts/loading-bar.min.js",
                       "~/Scripts/mask.min.js",
                       "~/Scripts/Principal.js"
                       ));
            
            bundles.Add(new ScriptBundle("~/listar").Include(
                       "~/Scripts/Sistemas/listarSistema.js",
                       "~/Scripts/Sistemas/cadastrarSistema.js",
                       "~/Scripts/Sistemas/editarSistema.js"
                       ));
            
            bundles.Add(new StyleBundle("~/cssStyles").Include(
                    "~/Content/loading-bar.min.css",
                    "~/Content/ui-bootstrap-csp.css",
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap.theme.min.css",
                    "~/Content/Site.css"
                ));

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

            bundles.Add(new StyleBundle("~/padrao-inmetro")
              .Include("~/Content/pi-barra-rodape-sistema-internet.css",
              "~/pi-conteudo.css", "~/pi-dropdown-submenu.css"));
        }
    }
}
