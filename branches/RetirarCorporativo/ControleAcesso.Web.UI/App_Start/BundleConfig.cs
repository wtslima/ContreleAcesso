using System.Web;
using System.Web.Optimization;

namespace ControleAcesso.Web.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jsScripts").Include(
                       "~/Scripts/jquery-1.10.2.min.js",
                //"~/Scripts/angular.min.js",
                       "~/Scripts/bootstrap.min.js",
                //"~/Scripts/angular-route.min.js",
                //"~/Scripts/angular-animate.min.js",
                       "~/Scripts/loading-bar.min.js",
                       "~/Scripts/mask.min.js"));
                       //"~/Scripts/angular-ui/ui-bootstrap-tpls.min.js"));

            bundles.Add(new StyleBundle("~/cssStyles").Include(
                    "~/Content/loading-bar.min.css",
                    "~/Content/ui-bootstrap-csp.css",
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap.theme.min.css",
                    "~/Content/Site.css"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/padrao-inmetro")
                .Include("~/Content/pi-barra-rodape-sistema-internet.css",
                "~/pi-conteudo.css", "~/pi-dropdown-submenu.css"));
        	
        	bundles.Add(new ScriptBundle("~/ControleAcesso")
        	            .Include("~/Modules/ControleAcesso.js",
                                "~/Modules/Utils/Utils.js",
                                "~/Modules/Utils/Utils.Filters.js",
                                "~/Modules/Utils/Utils.Directives.js",
                                "~/Modules/ControleAcesso.Login.Controller.js",
        	                    "~/Modules/Home/HomeController.js",
        	                    "~/Modules/Empresas/EmpresasController.js"));
        }
    }
}
