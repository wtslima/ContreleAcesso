
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Interfaces.Repositorio
{
    public interface ISistemaRepositorio : IRepositorio<Sistema>
    {
        Sistema Cadastrar(Sistema sistema);
        Sistema Atualizar(Sistema sistema);
    }
}