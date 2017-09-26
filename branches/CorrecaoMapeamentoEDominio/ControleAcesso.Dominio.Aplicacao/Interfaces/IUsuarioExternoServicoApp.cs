using System;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface IUsuarioExternoServicoApp : IServicoApp<UsuarioExterno>
    {
        UsuarioExterno Autenticar(int sistema, string login, string senha);
        void Cadastrar(UsuarioExterno usuario);
        string SolicitarSenhaTemporaria(string login, DateTime? dataExpiracao = null);
        bool CriarSenha(string login, string senha);
        bool AlterarSenha(string login, string senhaAtual, string senhaNova);

    }
}