using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace CONTROLE_DE_ACESSO_WEB.Apresentacao.Web.Windsor
{
    public class ControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Get e Set o container.
        /// </summary>
        public IWindsorContainer Container { get; protected set; }

        /// <summary>
        /// Inicializa uma nova instancia da classe
        /// </summary>
        /// <param name="container">O container usado para resolver os controllers do MVC</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public ControllerFactory(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            this.Container = container;
        }

        /// <summary>
        /// Recupera a instancia do controller para para o contexto do request especificado e o tipo do controller
        /// </summary>
        /// <param name="requestContext">O contexto do HTTP request, o qual inclui HTTP context e a rota dos dados</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// A instancia do controller.
        /// </returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            // Retrieve the requested controller from Castle
            return Container.Resolve(controllerType) as IController;
          }

        /// <summary>
        /// Libera o controller especificado.
        /// </summary>
        /// <param name="controller">O controller a ser liberado.</param>
        public override void ReleaseController(IController controller)
        {
            //Se o controller implementa IDisposable, limpa a responsabilidade
            var disposableController = controller as IDisposable;
            if (disposableController != null)
            {
                disposableController.Dispose();
            }

            //Informa ao castle do o controller não é mais necessário
            Container.Release(controller);
        }
    }
}