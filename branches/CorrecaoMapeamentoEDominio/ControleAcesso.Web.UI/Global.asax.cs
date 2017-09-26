using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ControleAcesso.Web.UI.App_Start;
using ControleAcesso.Web.UI.Windsor;

namespace ControleAcesso.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private WindsorContainer _container;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            Application["VersionNumber"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Application["VersionDate"] = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy");
        }

        protected void Application_End()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        private void InicializarWindsor()
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(
                new CastleControllerFactory(_container.Kernel));
        }
       
    }
}
