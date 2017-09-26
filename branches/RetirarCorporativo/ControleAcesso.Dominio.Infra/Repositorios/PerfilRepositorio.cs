using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces;
using NHibernate;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class PerfilRepositorio : Repositorio<Perfil>, IPerfilRepositorio
    {
        public PerfilRepositorio(ISession session) : base(session)
        {
        }
    }
}
