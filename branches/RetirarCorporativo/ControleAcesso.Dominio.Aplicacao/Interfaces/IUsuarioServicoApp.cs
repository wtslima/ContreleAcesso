using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface IUsuarioServicoApp : IServicoApp<Usuario>
    {
        Usuario AutenticarUsuario(string codigoSistema, string login, string senha);
    }
}