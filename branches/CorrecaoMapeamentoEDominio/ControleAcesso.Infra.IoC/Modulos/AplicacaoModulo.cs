using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Aplicacao.Servicos;

namespace ControleAcesso.Infra.IoC.Modulos
{
    public class AplicacaoModulo : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IServicoApp<>))
                .ImplementedBy(typeof(BaseServico<>))
                .LifestyleTransient());

            container.Register(Component.For(typeof(IUsuarioServicoApp))
                .ImplementedBy(typeof(UsuarioServico))
                .LifestyleTransient());

            container.Register(Component.For(typeof(IUsuarioExternoServicoApp))
                .ImplementedBy(typeof(UsuarioExternoServico))
                .LifestyleTransient());

            container.Register(Component.For(typeof(ISistemaServicoApp))
                .ImplementedBy(typeof(SistemaServico))
                .LifestyleTransient());

            container.Register(Component.For(typeof (IPerfilServicoApp))
                .ImplementedBy(typeof (PerfilServico))
                .LifestyleTransient());

            container.Register(Component.For(typeof(ISistemaPerfilServicoApp))
               .ImplementedBy(typeof(SistemaPerfilServico))
               .LifestyleTransient());

        }
    }
}