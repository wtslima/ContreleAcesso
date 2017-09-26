using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace ControleAcesso.Web.UI.Windsor
{
    public class CastleControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public CastleControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("O caminho do controller '{0}' não foi encontrado.", requestContext.HttpContext.Request.Path));
            }

            return _kernel.Resolve(controllerType) as IController;
        }

        public override void ReleaseController(IController controller)
        {
           _kernel.ReleaseComponent(controller);
        }
    }
}