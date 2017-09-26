using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface IUsuarioServicoApp : IServicoApp<Usuario>
    {
        Usuario AutenticarUsuario(int codigoSistema, string login, string senha);
    }
}