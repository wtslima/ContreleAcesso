using Castle.Windsor;
using ControleAcesso.Infra.IoC.Modulos;

namespace ControleAcesso.Infra.IoC
{
    public class ConfigurarDependencias
    {
        public virtual void Install(IWindsorContainer container)
        {
            container.Install(
                new AplicacaoModulo(),
                new InfraModulo()
                );
        }
    }
}