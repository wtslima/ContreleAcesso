using System.Linq;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class UsuarioExternoRepositorio : Repositorio<UsuarioExterno>
    {
        public override void Salvar(UsuarioExterno objeto)
        {
            var session = this.Conexao.ObterSessao();
            session.Persist(objeto);
            objeto.Perfis.ToList().ForEach(session.Persist);
            try
            {
                session.SaveOrUpdate((object)objeto);
                session.Flush();
            }
            catch
            {
                session.Evict(objeto);
                throw;
            }
        }
    }
}