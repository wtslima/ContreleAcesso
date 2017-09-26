using System.Linq;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class UsuarioExternoRepositorio : Repositorio<UsuarioExterno>
    {
        public override void Salvar(UsuarioExterno objeto)
        {
            var session = this.Conexao.ObterSessao();
            try
            {
                session.SaveOrUpdate(objeto);
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