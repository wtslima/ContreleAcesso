using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface ISistemaServicoApp : IServicoApp<Sistema>
    {
        bool ValidarToken(string token);
        ServidorOrigem DecomporToken(string token);
        Sistema BuscarPorToken(string token);
        void Excluir(string codigoSistema);
        void Cadastrar(Sistema objeto);

    }
}