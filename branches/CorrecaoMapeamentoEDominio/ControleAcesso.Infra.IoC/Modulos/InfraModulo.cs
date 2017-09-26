using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ControleAcesso.Dominio.Infra.Repositorios;
using ControleAcesso.Dominio.Interfaces;
using ControleAcesso.Dominio.Interfaces.Repositorio;

namespace ControleAcesso.Infra.IoC.Modulos
{
    public class InfraModulo : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof (IRepositorio<>))
                .ImplementedBy(typeof (Repositorio<>))
                .LifestyleTransient());

            container.Register(Component.For(typeof (IUsuarioRepositorio))
                .ImplementedBy(typeof (UsuarioRepositorio))
                .LifestyleTransient());

            container.Register(Component.For(typeof (IUsuarioExternoRepositorio))
                .ImplementedBy(typeof (UsuarioExternoRepositorio))
                .LifestyleTransient());

            container.Register(Component.For(typeof (ISistemaRepositorio))
                .ImplementedBy(typeof (SistemaRepositorio))
                .LifestyleTransient());


            container.Register(Component.For(typeof (IPerfilRepositorio))
                .ImplementedBy(typeof (PerfilRepositorio))
                .LifestyleTransient());

            container.Register(Component.For(typeof(ISistemaPerfilRepositorio))
                .ImplementedBy(typeof(SistemaPerfilRepositorio))
                .LifestyleTransient());

            container.AddFacility<PersistenciaFacility>();

        }

    }
}