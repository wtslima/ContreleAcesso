using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Web.Helpers;
using Castle.Windsor;
using ControleAcesso.Infra.IoC;
using CONTROLE_DE_ACESSO_WEB.Apresentacao.Web.Windsor;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CONTROLE_DE_ACESSO_WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InjecaoDepedencia();
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            Application["VersionNumber"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Application["VersionDate"] = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy");
        }

        private void InjecaoDepedencia()
        {
            // Inicializa e injeta as dependências de todos os projetos da aplicação.
            var injecao = new ConfigurarDependencias();
            injecao.Install(new WindsorContainer());// Injetar();

            // Registra os controllers do MVC na mesma instancia das dependencias da infra.
            var container = injecao.GetInstance();
            container.Install(new ControllerInstaller());

            // Cria a fabrica de controllers
            var ControllerFactory = new ControllerFactory(container);

            // Adciona a fabrica de controlles no pipeline do MVC web request.
            ControllerBuilder.Current.SetControllerFactory(ControllerFactory);
        }
    }
}
