using Castle.Windsor;
using ControleAcesso.Infra.IoC.Modulos;

namespace ControleAcesso.Infra.IoC
{
    public class ConfigurarDependencias
    {
        private IWindsorContainer _container;
        public virtual void Install(IWindsorContainer container)
        {
            container.Install(
                new AplicacaoModulo(),
                new InfraModulo()
                );
            _container = container;
        }

        public IWindsorContainer GetInstance()
        {
            return _container;
        }
    }
}