using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using User = ControleAcesso.Dominio.Entidades.Usuario;
using UserExterno = ControleAcesso.Dominio.Entidades.UsuarioExterno;
using ControleAcessoService.DataContracts;
using ControleAcesso.Dominio.Exceptions;
using System.Security.Cryptography;
using ControleAcesso.Dominio.Entidades;



namespace ControleAcesso.Teste.AutenticacaoWebService
{
    [TestFixture]
    public class AutenticacaoTeste
    {
        #region AutenticarUsuario

        protected string senha = "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B";
        [TestCase("SERVIR", "flavio.gentil@gmail.com", "Flávio Luis Pereira Gentil", TestName = "AUTENTICAR USUÁRIO EXTERNO - CASO DE QUEBRA", Category = "Autenticacao,Aplicação", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("SERVIR", "flgentil-cast@inmetro.gov.br", "Flávio Luis Pereira Gentil", TestName = "AUTENTICAR USUÁRIO EXTERNO - SUCESSO", Category = "Autenticacao,Aplicação")]
        public void AutenticarUsuarioExterno(string sistema, string login, string nome)
        {
            UserExterno usuario = UsuarioExternoServico.Instancia.Autenticar(sistema, login, senha);
            Assert.AreEqual(usuario.Nome, nome);
            Assert.IsNotNull(usuario);
        }

        [TestCase("SERVIR", "flavio.gentil@gmail.com", "Flávio Luis Pereira Gentil", TestName = "AUTENTICAR USUÁRIO INTERNO - CASO DE QUEBRA", Category = "Autenticacao,Aplicação", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("SERVIR", "flgentil-cast@inmetro.gov.br", "Flávio Luis Pereira Gentil", TestName = "AUTENTICAR USUÁRIO INTERNO - SUCESSO", Category = "Autenticacao,Aplicação")]
        public void AutenticarUsuarioInterno(string sistema, string login, string nome)
        {
            User usuario = UsuarioServico.Instancia.AutenticarUsuario(sistema, login, senha);
            Assert.AreEqual(usuario.Nome, nome);
            Assert.IsNotNull(usuario);
        }

        [TestCase("0ee2f272b9afa70005457dee28c86693", "flavio.gentil@gmail.com", "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B", "Flávio", TestName = "AUTENTICAR USUÁRIO - CASO DE QUEBRA", Category = "Autenticacao,WebService", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("0ee2f272b9afa70005457dee28c86693", "flgentil-cast@inmetro.gov.br", "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B", "Flávio", TestName = "AUTENTICAR USUÁRIO - CASO DE SUCESSSO", Category = "Autenticacao,WebService")]
        public void AutenticarUsuario(string token, string usuario, string senha, string nome)
        {
            var servico = new ControleAcessoService.AutenticacaoServico();

            var login = new Login
            {
                Senha = senha,
                UserName = usuario
            };

            var user = servico.Autenticar(token, login);

            Assert.AreEqual(user.Nome, nome);
            Assert.IsNotNull(user);
        }

        #endregion

        #region CriaUsuario

        [TestCase("flgentil", "Flávio Luis Pereira Gentil", "flavio.gentil@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO SEM ENVIO DE SENHA TEMPORÁRIA - CASO DE QUEBRA", Category = "Criar Usuário, Aplicação", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("wellingtonts.lima@gmail.com", "Wellington Silva Lima", "wellingtonts.lima@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO SEM ENVIO DE SENHA TEMPORÁRIA - SUCESSSO", Category = "Criar Usuário, Aplicação", ExpectedException = typeof(LoginCadastradoException))]
        [TestCase("bongoboy7@gmail.com", "Flávio Luis Pereira Gentil", "bongoboy7@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO SEM ENVIO DE SENHA TEMPORÁRIA - SUCESSSO", Category = "Criar Usuário, Aplicação", ExpectedException = typeof(LoginCadastradoException))]
        public void CriarUsuarioSemEnvioSenhaTemporaria(string login, string nome, string email, int idPessoaFisica, string senha, string codigoPerfil, string codigoSistema)
        {
           
            var usuario = new UserExterno
            {
                Login = login,
                Ativo = true,
                Email = email,
                IdPessoaFisica = idPessoaFisica,
                Nome = nome,
                Origem = "I",
                Uso = "I",
                Alteracao = DateTime.Now

            };
            usuario.AdicionarPerfil(new ControleAcesso.Dominio.Entidades.SistemaPerfil { CodigoSistema = "SERVIR", CodigoPerfil = "AUTENTIC" });
            usuario.CriarSenha(senha);
            usuario.AdicionarSenha(new ControleAcesso.Dominio.Entidades.UsuarioExternoSenha(login, "", DateTime.Now.AddMonths(1), true));
            UsuarioExternoServico.Instancia.Salvar(usuario);

            var usu = UsuarioExternoServico.Instancia.Buscar(x => x.Login.Equals(login)).FirstOrDefault();

            Assert.AreEqual(usu.Login, login);

        }


        [TestCase("flgentil", "Flávio Luis Pereira Gentil", "flavio.gentil@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO COM ENVIO DE SENHA TEMPORÁRIA - CASO DE QUEBRA", Category = "Criar Usuário, WebService", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("wellingtonts.lima@gmail.com", "Wellington Silva Lima", "wellingtonts.lima@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO COM ENVIO DE SENHA TEMPORÁRIA - SUCESSSO", Category = "Criar Usuário, WebService", ExpectedException = typeof(LoginCadastradoException))]
        [TestCase("bongoboy7@gmail.com", "Flávio Luis Pereira Gentil", "bongoboy7@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO COM ENVIO DE SENHA TEMPORÁRIA - SUCESSSO", Category = "Criar Usuário, WebService", ExpectedException = typeof(LoginCadastradoException))]
        public void CriarUsuarioComEnvioSenhaTemporaria(string token, string senha, string login, string email, string nome, int idPessoaFisica)
        {
            var servico = new ControleAcessoService.AutenticacaoServico();

            var usuario = new UserExterno
            {
                Login = login,
                Ativo = true,
                Email = email,
                IdPessoaFisica = idPessoaFisica,
                Nome = nome,
                Origem = "I",
                Uso = "I",
                Alteracao = DateTime.Now

            };

            var user = servico.CriarUsuarioExterno(token, usuario);

            Assert.AreEqual(usuario.Nome, nome);
            Assert.IsNotNull(user);
                       
        }
        #endregion

        #region Senha

        [TestCase("dcbelmont@inmetro.gov.br", "123mudar", "123mudou?", TestName = "ALTERAR SENHA DE USUÁRIO EXTERNO - CASO DE SUCESSO", Category = "Senha Usuário, Aplicação", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("dcbelmont@inmetro.gov.br", "senhaTemporaria", "123mudarei", TestName = "ALTERAR SENHA DE USUÁRIO EXTERNO - CASO DE QUEBRA", Category = "Senha Usuário, Aplicação")]
        public void AlterarSenhaUsuarioExterno(string login, string senhaAtual, string senhaNova)
        {
            senhaAtual = senhaAtual.Equals("senhaTemporaria") ? UsuarioExternoServico.NovaInstancia.SolicitarSenhaTemporaria(login) : senhaAtual;
            Assert.IsTrue(UsuarioExternoServico.NovaInstancia.AlterarSenha(login, Criptografar(senhaAtual), Criptografar(senhaNova)));
        }

        [TestCase("flgentil-cast@inmetro.gov.br", TestName = "SOLICITAR SENHA TEMPORÁRIA - CASO DE SUCESSO", Category = "Senha Usuário, Aplicação")]
        [TestCase("flgentil", TestName = "SOLICITAR SENHA TEMPORÁRIA - CASO DE QUEBRA", Category = "Senha Usuário, Aplicação", ExpectedException = typeof(SenhaTemporariaException))]
        public void SolicitarSenhaTemporaria(string login, DateTime? dataExpiracao = null)
        {
            var servico = UsuarioExternoServico.NovaInstancia;
            var senhaTemporaria = servico.SolicitarSenhaTemporaria(login);

            var senha = Servico<UsuarioExternoSenha>
                .Instancia
                .Buscar(s => s.Login.ToLower().Equals(login.ToLower()) && s.Tipo.Equals("T") && s.Ativo == true && s.Expiracao.CompareTo(DateTime.Now) > 0)
                .FirstOrDefault();
            Assert.IsNotNull(senha);
            Assert.IsTrue(servico.ValidarSenha(senha.UsuarioExterno, Criptografar(senhaTemporaria)).IsTemporaria);
            Assert.AreEqual(login.ToLower(), senha.Login.ToLower());
        }

        #endregion

        #region BuscarUsuario

        [TestCase("flgentil-cast@inmetro.gov.br", "Flávio Luis Pereira Gentil", TestName = "BUSCAR USUARIO EXTERNO - CASO DE SUCESSO", Category = "Buscar Usuário, WebService")]
        [TestCase("flgentil", "Flávio Luis Pereira Gentil", TestName = "BUSCAR USUARIO EXTERNO - CASO DE QUEBRA", Category = "Senha Usuário, Aplicação", ExpectedException = typeof(LoginInexistenteException))]
        public void BuscarUsuarioExterno(string login, string nome)
        {
            var usuario = UsuarioServico.Instancia.Buscar(u => u.Login.Equals(login)).First();
            Assert.IsNotNull(usuario);
            Assert.AreEqual(usuario.Nome.ToLower(), nome);
        }


        [TestCase("0ee2f272b9afa70005457dee28c86693", 0, "false", TestName = "OBTER TODOS USUÁRIOS - TODOS", Category = "Buscar Usuário, WebService")]
        [TestCase("0ee2f272b9afa70005457dee28c86693", 1, "false", TestName = "OBTER TODOS USUÁRIOS - INTERNOS", Category = "Buscar Usuário, WebService")]
        [TestCase("0ee2f272b9afa70005457dee28c86693", 2, "false", TestName = "OBTER TODOS USUÁRIOS - EXTERNOS", Category = "Buscar Usuário, WebService")]
        public void ObterTodosUsuarios(string token, int filtro, bool filtrarPorSistema)
        {
            var servico = new ControleAcessoService.AutenticacaoServico();

            var usuario = servico.ObterTodosUsuarios(token, filtro, filtrarPorSistema);

            Assert.IsNotNull(usuario);
        }

        #endregion

        private static string Criptografar(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] value = new SHA256Managed().ComputeHash(bytes);
            return BitConverter.ToString(value);
        }
    }
}