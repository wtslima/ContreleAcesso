using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using ControleAcesso.Dominio.Infra.Mapeamentos;
using NHibernate.Context;
using NHCfg = NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace ControleAcesso.Infra.IoC.Modulos
{
    public class PersistenceFacilityTest : AbstractFacility
    {
        private static ISessionFactory _factory = null;
        protected override void Init()
        {
            Kernel.Register(
               Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory).LifestyleSingleton(),
               Component.For<ISession>().UsingFactoryMethod(OpenSession).LifeStyle.PerThread
           );
        }

        private static ISessionFactory CreateSessionFactory()
        {
            NHibernate.Cfg.Configuration config = Fluently.Configure()
                .Database(CreateDbConfig)
                .CurrentSessionContext("call")
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsuarioMapa>())
                .BuildConfiguration();

            _factory = config.BuildSessionFactory();

            return _factory;
        }

        private static ISession OpenSession(IKernel kernel)
        {
            var factory = kernel.Resolve<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(factory))
            {
                CurrentSessionContext.Bind(factory.OpenSession());
            }
            var session = factory.GetCurrentSession();
           
            return session;
        }

        private static MsSqlConfiguration CreateDbConfig()
        {
            return MsSqlConfiguration
                .MsSql2008
                .ConnectionString(c => c.FromConnectionStringWithKey("INMETRO"));
        }




    }
}