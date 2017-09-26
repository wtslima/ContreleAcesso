using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcessoService;
using ControleAcessoService.DataContracts;
using ControleAcessoService.DataContracts;
using NUnit.Framework;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class UsuarioTestes
	{
		public UsuarioTestes() {
			log4net.Config.XmlConfigurator.Configure();
		}

        public  string UserHostName { get; set; }

        protected string _token = ConfigurationManager.AppSettings["token"];


		#region AutenticarUsuario
        [TestCase("SIGRH", "fulaninho", "123", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("SIGRH", "dcbelmont", "123", ExpectedException = typeof(SenhaInvalidaException))]
        [TestCase("SIGRH", "svnappadmin", "inmetro@0909", ExpectedException = typeof(LoginNaoAssociadoAPessoaException))]
        /*TODO: Acrescentar caso de teste para login válido
 		* [TestCase("SIGRH", "dcbelmont", "xpto")]*/
        //TODO: Acrescentar caso de teste para usuário sem perfil de acesso a sistema

        [TestCase("0ee2f272b9afa70005457dee28c86693", "flgentil-cast@inmetro.gov.br", "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B", "Flávio", TestName = "AUTENTICAR USUÁRIO - SUCESSSO", Category = "USUARIOAUTENTICAR")]
        public void AutenticarUsuarioTeste(string token, string usuario, string senha, string nome)
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

        [TestCase("eee2deb1d34bb3973c5799b33421627", "dbelmont@gmail.com", "5B-A8-AB-0A-0E-ED-49-2A-0E-1B-79-E1-20-C0-E9-BF-6C-D4-91-0D-E8-FB-A6-51-A2-EC-3A-0B-2A-C4-28-9D", TestName = "Autenticar Usuario Externo")]
        public void Autenticar(string token, string usuario, string senha)
        {
            var servico = new ControleAcessoService.AutenticacaoServico();
          
            var login = new Login
            {
                Senha = senha,
                UserName = usuario
            };

          var user =  servico.Autenticar(token, login);
          Assert.IsNotNull(user);
        }

		#endregion
		
		/*[Test]
		public void AtribuirPerfilUsuario() {
			var usuario = UsuarioServico.Instancia.Buscar(u => u.Login.Equals("dcbelmont")).First();
			var perfil = new UsuarioSistemaPerfil {
				LoginUsuario = "dcbelmont",
				CodigoSistema = "PONTOFOCAL",
				CodigoPerfil = "AUTENTIC",
				Ativo = false,
				Alteracao = DateTime.Now
			};
			
			usuario.AdicionarPerfil(perfil);
			UsuarioServico.NovaInstancia.Salvar(usuario);
		}*/
		
		[Test]
		public void BuscarUsuarioEspecifico_Sucesso() {
			var usuario = UsuarioServico.Instancia.Buscar(u => u.Login.Equals("dcbelmont")).First();
			Assert.IsNotNull(usuario);
			Assert.AreEqual(usuario.Nome.ToLower(), "david costa belmont");
		}
		
		[Test]
		public void UsuarioAssociadoPerfil_Sucesso() {
			var usuario = UsuarioServico.Instancia.Buscar(u => u.Login.Equals("dcbelmont")).First();
			var perfil = usuario.Perfis.Where(p => p.CodigoPerfil.Trim().Equals("AUTENTIC")).Single();
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			Assert.AreEqual(comparer.Compare(perfil.ToString(), "[UsuarioSistemaPerfil Usuario=DAVID COSTA BELMONT, Perfil=[SistemaPerfil CodigoSistema=SIGRH, CodigoPerfil=AUTENTIC  ]]"), 0);
		}
        
	}
}