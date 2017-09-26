using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;

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

        public UsuarioExterno Autenticar(string codigoSistema, string login, string senha)
        {
            var lista = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim()));
            if (!lista.Any()) throw new LoginInexistenteException(login);
            var usuario = lista.First();

            if (!usuario.Ativo)
                throw new AcessoNegadoException();

            if (!(usuario.Perfis.Any(p => p.Ativo) && usuario.Perfis.Any(p => p.CodigoPerfil.Trim().Equals("AUTENTIC"))))
                throw new AcessoNegadoException();

            try
            {
                var usuarioExternoSenha = ValidarSenha(usuario, senha);
                var resultado =
                    _repositorioUsuarioSenha.Buscar(s => s.Login.Equals(login) && s.Tipo.Equals("P") && s.Ativo).FirstOrDefault();
                if(resultado == null)
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
                    && us.Ativo).FirstOrDefault();

                if (usuario == null) throw;
                
                if (usuario.IdPessoaFisica == objeto.IdPessoaFisica)
                    throw new UsuarioCadastradoException();

                if (usuario.Login.Trim().ToUpper() == objeto.Login.Trim().ToUpper())
                    throw new LoginCadastradoException();
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
                var senha = ValidarSenha(usuario, senhaAtual);

                if (senha == null)
                {
                    throw new SenhaInvalidaException();
                }

                if (senha.IsTemporaria)
                {
                    _repositorioUsuarioSenha.Buscar(x => x.Login.Equals(usuario.Login) && senha.Tipo.Equals("T"));
                    _repositorioUsuarioSenha.Excluir(senha);
                }

                var usuarioSenha = new UsuarioExternoSenha(login, senhaNova) { UsuarioExterno = usuario };

                _repositorioUsuarioSenha.Salvar(usuarioSenha);
            }
            else
            {
                throw new AcessoNegadoException();
            }

            return true;
        }

        public bool CriarSenha(string login, string senha)
        {
            var usuario = Buscar(u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim())).FirstOrDefault();
            if (usuario != null)
            {

                if (string.IsNullOrWhiteSpace(senha))
                {
                    throw new SenhaInvalidaException();
                }
                var pwd = Criptografar(senha);

                var usuarioExternoSenha = new UsuarioExternoSenha(login, pwd, null, false);
                usuarioExternoSenha.UsuarioExterno = usuario;

                _repositorioUsuarioSenha.Salvar(usuarioExternoSenha);
            }
            else
            {
                throw new AcessoNegadoException();
            }

            return true;
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
            var novaSenha = Criptografar(senhaAleatoria);

            DateTime prazoSenha;

            if (dataExpiracao == null)
            {
                prazoSenha = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"])).Date;
                prazoSenha = prazoSenha.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                prazoSenha = dataExpiracao.Value;
            }

            var usuarioExternoSenha = _repositorioUsuarioSenha.Buscar(p =>
                    p.Login.Trim().ToLower().Equals(login.Trim().ToLower()) && p.Tipo.Equals("P")).FirstOrDefault();
            if (usuarioExternoSenha != null)
                DesativarSenhaPermanente(login);

           

            var senha = new UsuarioExternoSenha(login, novaSenha, prazoSenha, true);

            if (usuario != null)
                senha.UsuarioExterno = usuario;

            _repositorioUsuarioSenha.Salvar(senha);
            return senhaAleatoria;
        }

        private void DesativarSenhaPermanente(string login)
        {
            if (!string.IsNullOrWhiteSpace(login))
            {
                var servico = _repositorioUsuarioSenha.Buscar(p =>
                    p.Login.Trim().ToLower().Equals(login.Trim().ToLower()) && p.Tipo.Equals("P")).FirstOrDefault();
                _repositorioUsuarioSenha.Excluir(servico);

            }
        }

        private static string CriarSenhaAleatoria(int tamanhoSenha)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789!@$?_-ABCDEFGHJKLMNOPQRSTUVWXYZ";
            char[] chars = new char[tamanhoSenha];
            Random rd = new Random();

            for (int i = 0; i < tamanhoSenha; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
        private static string Criptografar(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] value = new SHA256Managed().ComputeHash(bytes);
            return BitConverter.ToString(value);
        }
    }
}