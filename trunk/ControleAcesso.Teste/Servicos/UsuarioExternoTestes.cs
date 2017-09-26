using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Exceptions;
using Corporativo.Models;
using Inmetro.Utils.Linq.ExtensionMethods;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
    [TestFixture]
    public class UsuarioExternoTestes
    {
    	public const string login = "flgentil-cast@inmetro.gov.br";
    	
        public UsuarioExternoTestes() {
            log4net.Config.XmlConfigurator.Configure();
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            //Autenticacao_AlterarSenha(login, "senhaTemporaria", "123mudar");
        }
        
        [TestFixtureTearDown]
        public void TearDownFixture() {
            //Autenticacao_AlterarSenha(login, "senhaTemporaria", "123mudar");
        }

        [TestCase("SERVIR", "dcbelmont@inmetro.gov.br", "123mudarei")]
        [TestCase("SERVIR", "dcbelmont@inmetro.gov.br", "", ExpectedException = typeof(SenhaInvalidaException))]
        [TestCase("SERVIR", "dcbelmont@gmail.com", "", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("SIGRH", "dcbelmont@inmetro.gov.br", "123mudarei", ExpectedException = typeof(AcessoNegadoException))]
        [TestCase("SERVIR", "dcbelmont@inmetro.gov.br", "senhaTemporaria", ExpectedException = typeof(SenhaTemporariaException))]
        public void Autenticar(string sistema, string login, string senha)
        {
            senha = senha.Equals("senhaTemporaria") ? UsuarioExternoServico.NovaInstancia.SolicitarSenhaTemporaria(login) : senha;
            var usuario = UsuarioExternoServico.Instancia.Autenticar(sistema, login, Criptografar(senha));
            Assert.AreEqual(usuario.Login.ToLower(), login.ToLower());
        }
        
        [Test]
        [ExpectedException(typeof(SenhaTemporariaExpiradaException))]
        public void AutenticarSenhaTemporariaExpirada() {
        	//Expirando a senha temporária...
            var servicoUsuario = UsuarioExternoServico.NovaInstancia;
            var senhaTemporaria = servicoUsuario.SolicitarSenhaTemporaria(login);
            var usuario = servicoUsuario.Buscar(u => u.Login.ToLower().Equals(login)).FirstOrDefault();
            usuario.SenhaTemporaria.Expiracao = DateTime.Now.AddDays(-14);
            servicoUsuario.SalvarComTransacao(usuario);
        	
        	servicoUsuario.Autenticar("SERVIR", login, Criptografar(senhaTemporaria));
        }

        [TestCase("dcbelmont@inmetro.gov.br", "123mudar", "123mudou?")]
        [TestCase("dcbelmont@inmetro.gov.br", "senhaTemporaria", "123mudarei")]
        public void Autenticacao_AlterarSenha(string login, string senhaAtual, string senhaNova) {
            senhaAtual = senhaAtual.Equals("senhaTemporaria") ? UsuarioExternoServico.NovaInstancia.SolicitarSenhaTemporaria(login) : senhaAtual;
        	Assert.IsTrue(UsuarioExternoServico.NovaInstancia.AlterarSenha(login, Criptografar(senhaAtual), Criptografar(senhaNova)));
        }

        [Test]
        public void SolicitarSenhaTemporaria() {
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

        private static string Criptografar(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] value = new SHA256Managed().ComputeHash(bytes);
            return BitConverter.ToString(value);
        }

        [TestCase("dbelmont@gmail.com", 1, TestName="Cadastro de usuário com login existente (FALHA)", ExpectedException=typeof(LoginCadastradoException))]
        [TestCase("esteloginnaoexistemesmo@gmail.com", 7064, TestName="Cadastro de usuário que já possui login (FALHA)", ExpectedException=typeof(UsuarioCadastradoException))]
        public void CriarUsuarioExistente(string login, int idPessoaFisica)
        {
            var usuario = new UsuarioExterno
            {
                Login = login,
                Ativo = true,
                Email = login,
                IdPessoaFisica = idPessoaFisica,
                Nome = "Teste",
                Origem = "I",
                Uso = "I",
                Alteracao = DateTime.Now
            };
            usuario.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "SERVIR", CodigoPerfil = "AUTENTIC" });

            UsuarioExternoServico.Instancia.Salvar(usuario);
        }
        [TestCase("wellingtonts.lima@gmail.com", "Wellington Silva Lima", "wellingtonts.lima@gmail.com", 9358, "123mudar", "AUTENTIC", "SERVIR", TestName = "CRIAR USUÁRIO EXTERNO SEM ENVIO DE SENHA TEMPORÁRIA - SUCESSSO", Category = "USUARIOEXTERNO")]
        public void CriarUsuarioSemEnvioSenhaTemporaria(string login, string nome, string email, int idPessoaFisica, string senha, string codigoPerfil, string codigoSistema)
        {
            var usuario = new UsuarioExterno
            {
                Login = login,
                Ativo = true,
                Email = email,
                IdPessoaFisica = idPessoaFisica,
                Nome = nome,
                Origem = "I",
                Uso ="I",
                Alteracao = DateTime.Now

            };
            usuario.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "SERVIR", CodigoPerfil = "AUTENTIC" });
            usuario.CriarSenha(senha);
            usuario.AdicionarSenha(new UsuarioExternoSenha(login, "", DateTime.Now.AddMonths(1), true));
            UsuarioExternoServico.Instancia.Salvar(usuario);

            var usu = UsuarioExternoServico.Instancia.Buscar(x => x.Login.Equals(login)).FirstOrDefault();

            Assert.AreEqual(usu.Login, login);

        }

    }
}