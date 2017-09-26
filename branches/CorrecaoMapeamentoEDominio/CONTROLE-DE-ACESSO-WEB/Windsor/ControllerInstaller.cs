using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CONTROLE_DE_ACESSO_WEB.Controllers;

namespace CONTROLE_DE_ACESSO_WEB.Apresentacao.Web.Windsor
{
    public class ControllerInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Realiza o registro dos controllers da camada de aplicação" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
             container.Register(Classes
                .FromAssemblyContaining(typeof(BaseController<>))
                .BasedOn<Controller>()
                .LifestylePerWebRequest());
        }
    }
}