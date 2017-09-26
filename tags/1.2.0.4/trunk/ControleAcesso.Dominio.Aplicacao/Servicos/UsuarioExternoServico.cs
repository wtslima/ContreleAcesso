using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Infra.Repositorios;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    public class UsuarioExternoServico : Servico<UsuarioExterno>
    {
        public UsuarioExternoServico() : base()
        {
            Repositorio = new UsuarioExternoRepositorio();
        }

        public static new UsuarioExternoServico Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = (UsuarioExternoServico)new UsuarioExternoServico();
                }

                return (UsuarioExternoServico)_instancia;
            }
        }

        public static new UsuarioExternoServico NovaInstancia
        {
            get
            {
                _instancia = new UsuarioExternoServico();
                return (UsuarioExternoServico)_instancia;
            }
        }

        public UsuarioExterno Autenticar(string codigoSistema, string login, string senha)
        {
            var lista = UsuarioExternoServico.Instancia.Buscar(u => u.Login.ToLower().Equals(login.ToLower().Trim()));
            if (lista.Count() > 0)
            {
                var usuario = lista.First();

                if(!usuario.Ativo)
                    throw new AcessoNegadoException();

                if (!(usuario.Perfis.Any(p => p.Ativo) && usuario.Perfis.Any(p => p.CodigoPerfil.Trim().Equals("AUTENTIC"))))
                    throw new AcessoNegadoException();
                
                try
                {
                    var usuarioExternoSenha = ValidarSenha(usuario, senha); 

                    if (usuarioExternoSenha.IsTemporaria)
                        throw new SenhaTemporariaException();                    
                }
                catch (Dominio.Exceptions.SenhaTemporariaExpiradaException)
                {
                    var novaSenha = GerarSenhaTemporaria(login);
                    throw new Dominio.Exceptions.SenhaTemporariaExpiradaException("A senha temporária informada expirou. Uma nova senha foi enviada para seu e-mail.", novaSenha);
                }

                usuario.Contextualizar(codigoSistema); 

                return usuario;
            }
            else
            {
                throw new LoginInexistenteException(login);
            }
        }

        public override void Salvar(UsuarioExterno objeto)
        {
            try
            {
                base.Salvar(objeto);
            }
            catch
            {
                var usuario = Buscar(us => 
                    (us.Login.Trim().ToUpper() == objeto.Login.Trim().ToUpper() || us.IdPessoaFisica == objeto.IdPessoaFisica)
                    && us.Ativo == true).FirstOrDefault();
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

        public UsuarioExternoSenha ValidarSenha(UsuarioExterno usuario, string senha)
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
            var usuario = this.Buscar(u => u.Login.ToLower().Equals(login.ToLower().Trim())).FirstOrDefault();
            if (usuario != null)
            {
                var pwd = ValidarSenha(usuario, senhaAtual);

                if (pwd == null)
                {
                    throw new SenhaInvalidaException();
                }

                if (pwd.IsTemporaria)
                {
                    Servico<UsuarioExternoSenha>.Instancia.Excluir(s => s.Login.Equals(usuario.Login) && s.Tipo.Trim() == "T");
                }

                var usuarioExternoSenha = new UsuarioExternoSenha(login, senhaNova, null, false);
                usuarioExternoSenha.UsuarioExterno = usuario;

                var servico = Servico<UsuarioExternoSenha>.Instancia;
                servico.Salvar(usuarioExternoSenha);
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
            if (UsuarioExternoServico.Instancia.Buscar(u => u.Login.Trim().ToLower().Equals(loginUsuarioExterno.Trim().ToLower())).Count() == 0)
            {
                throw new LoginInexistenteException(loginUsuarioExterno);
            }

           return GerarSenhaTemporaria(loginUsuarioExterno, dataExpiracao);
        }


        public string GerarSenhaTemporaria(string login, DateTime? dataExpiracao = null)
        {
            
            var senhaAleatoria = CriarSenhaAleatoria(6);
            var novaSenha = Criptografar(senhaAleatoria);

            var prazoSenha = DateTime.MinValue;

            if (dataExpiracao == null)
            {
                prazoSenha = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"])).Date;
                prazoSenha = prazoSenha.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                prazoSenha = dataExpiracao.Value;
            }

            var senha = new UsuarioExternoSenha(login, novaSenha, prazoSenha, true);
            Servico<UsuarioExternoSenha>.Instancia.SalvarComTransacao(senha);
            return senhaAleatoria;
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