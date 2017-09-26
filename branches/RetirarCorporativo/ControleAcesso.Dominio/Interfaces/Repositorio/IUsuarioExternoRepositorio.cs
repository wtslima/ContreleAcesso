
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Interfaces.Repositorio
{
    public interface IUsuarioExternoRepositorio : IRepositorio<UsuarioExterno>
    {
        void Cadastrar(UsuarioExterno objeto);
    }
}