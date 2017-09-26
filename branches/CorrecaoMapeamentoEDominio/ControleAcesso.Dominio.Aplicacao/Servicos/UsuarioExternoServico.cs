using System;
using System.Configuration;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using FluentNHibernate.Conventions;
using NHibernate.Mapping;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    public class UsuarioExternoServico : BaseServico<UsuarioExterno>, IUsuarioExternoServicoApp
    {
        private readonly IRepositorio<UsuarioExternoSenha> _repositorioUsuarioSenha;
        private readonly IUsuarioExternoRepositorio _repositorio;

        public UsuarioExternoServico(IUsuarioExternoRepositorio repositorio, IRepositorio<UsuarioExternoSenha> repositarioUsuarioSenha)
            : base(repositorio)
        {
            _repositorioUsuarioSenha = repositarioUsuarioSenha;
            _repositorio = repositorio;
        }

        public UsuarioExterno Autenticar(int codigoSistema, string login, string senha)
        {
            var lista = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim()));
            if (!lista.Any()) throw new LoginInexistenteException(login);
            var usuario = lista.First();

            if (usuario.Excluido)
                throw new AcessoNegadoException();

            if ((usuario.Perfis.Any(p => p.Excluido) && usuario.Perfis.Any(p => p.IdPerfil.Equals(5))))
                throw new AcessoNegadoException();

            try
            {
                var usuarioExternoSenha = ValidarSenha(usuario, senha);
                var resultado =
                    _repositorioUsuarioSenha.Buscar(s => s.UsuarioExterno.Login.Equals(login) && s.Tipo.Equals("P") && s.Excluido == false).FirstOrDefault();
                if (resultado == null)
                {

                    if (usuarioExternoSenha.IsTemporaria)
                        throw new SenhaTemporariaException();
                }
            }
            catch (Dominio.Exceptions.SenhaTemporariaExpiradaException)
            {
                var novaSenha = GerarSenhaTemporaria(login);
                throw new Dominio.Exceptions.SenhaTemporariaExpiradaException("A senha temporária informada expirou. Uma nova senha foi enviada para seu e-mail.", novaSenha);
            }

            usuario.Contextualizar(codigoSistema);

            return usuario;
        }

        public void Cadastrar(UsuarioExterno objeto)
        {
            try
            {
                _repositorio.Cadastrar(objeto);
            }
            catch
            {
                var usuario = Buscar(us =>
                    (us.Login.Trim().ToUpper() == objeto.Login.Trim().ToUpper() || us.IdPessoaFisica == objeto.IdPessoaFisica)
                    && us.Excluido == false).FirstOrDefault();
                if (usuario != null)
                {
                    if (usuario.IdPessoaFisica == objeto.IdPessoaFisica)
                        throw new UsuarioCadastradoException();

                    if (usuario.Login.Trim().ToUpper() == objeto.Login.Trim().ToUpper())
                        throw new LoginCadastradoException();
                }

                throw;
            }
        }
        private UsuarioExternoSenha ValidarSenha(UsuarioExterno usuario, string senha)
        {
            var pwd = usuario.Autenticar(senha);
            if (pwd == null)
            {
                throw new SenhaInvalidaException();
            }
            return pwd;
        }
        public bool AlterarSenha(string login, string senhaAtual, string senhaNova)
        {
            var usuario = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim())).FirstOrDefault();
            if (usuario != null)
            {
                if (usuario.Autenticar(senhaAtual) != null)
                {
                    usuario.AdicionarSenha(senhaNova);
                    Salvar(usuario);
                    return true;
                }
                throw new SenhaInvalidaException();
            }
            throw new AcessoNegadoException();
        }

        public bool CriarSenha(string login, string senha)
        {
            var usuario = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim())).FirstOrDefault();
            if (usuario != null && usuario.Autenticar(senha) != null)
            {
                usuario.AdicionarSenha(senha);
                Salvar(usuario);
                return true;
            }
            throw new AcessoNegadoException();
        }

        public string SolicitarSenhaTemporaria(string loginUsuarioExterno, DateTime? dataExpiracao = null)
        {
            //Verifica se o usuário existe
            if (!Buscar(u => u.Login.Trim().ToLower().Equals(loginUsuarioExterno.Trim().ToLower())).Any())
            {
                throw new LoginInexistenteException(loginUsuarioExterno);
            }

            return GerarSenhaTemporaria(loginUsuarioExterno, dataExpiracao);
        }

        private string GerarSenhaTemporaria(string login, DateTime? dataExpiracao = null)
        {
            var usuario = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim())).FirstOrDefault();

            var senhaAleatoria = CriarSenhaAleatoria(6);

            if (usuario != null && dataExpiracao == null)
            {
                var prazoSenha = DateTime.Now.AddDays(
                        Convert.ToInt32(ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"])).Date;
                prazoSenha.AddHours(23).AddMinutes(59).AddSeconds(59);
                usuario.AdicionarSenha(senhaAleatoria, prazoSenha, true);
                Salvar(usuario);
            }

            return senhaAleatoria;
        }
       

        private static string CriarSenhaAleatoria(int tamanhoSenha)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789!@$?_-ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var chars = new char[tamanhoSenha];
            var rd = new Random();

            for (int i = 0; i < tamanhoSenha; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}