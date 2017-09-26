using Castle.Windsor;

namespace ControleAcesso.Infra.IoC
{
    public class InvercaoDeControle
    {
        private IWindsorContainer _container;

        public void Injetar()
        {
            _container = new WindsorContainer();
            ConfigurarDependencias.Install(_container);
        }

        public IWindsorContainer GetInstance()
        {
            return _container;
        }

        public T Resolver<T>()
        {
            return _container.Resolve<T>();
        }
 
    }
}