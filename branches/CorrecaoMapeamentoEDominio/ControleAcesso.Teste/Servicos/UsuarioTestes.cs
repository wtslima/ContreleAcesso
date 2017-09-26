using System;
using System.Configuration;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcessoService.DataContracts;
using log4net.Config;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class UsuarioTestes
	{
	    private UsuarioServico _servico;
		public UsuarioTestes(UsuarioServico servico)
		{
		    _servico = servico;
		    XmlConfigurator.Configure();
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
          //  var servico = new AutenticacaoServico();

            var login = new Login
            {
                Senha = senha,
                UserName = usuario
            };

            var user = _servico.AutenticarUsuario(2 , usuario, senha);

            Assert.AreEqual(user.Nome, nome);
            Assert.IsNotNull(user);
        }

        [TestCase("dbelmont@gmail.com", "5B-A8-AB-0A-0E-ED-49-2A-0E-1B-79-E1-20-C0-E9-BF-6C-D4-91-0D-E8-FB-A6-51-A2-EC-3A-0B-2A-C4-28-9D", TestName = "Autenticar Usuario Externo")]
        public void Autenticar(string token, string login, string senha)
        {
          //  var servico = new AutenticacaoServico();
          
          var user =  _servico.AutenticarUsuario(1, login, senha);
          Assert.IsNotNull(user);
        }

		#endregion
		
		/*[Test]
		public void AtribuirPerfilUsuario() {
			var usuario = _servico.Buscar(u => u.Login.Equals("dcbelmont")).First();
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
		public void BuscarUsuarioEspecificoSucesso() {
			var usuario = _servico.Buscar(u => u.Login.Equals("dcbelmont")).First();
			Assert.IsNotNull(usuario);
			Assert.AreEqual(usuario.Nome.ToLower(), "david costa belmont");
		}
		
		[Test]
		public void UsuarioAssociadoPerfilSucesso() {
			var usuario = _servico.Buscar(u => u.Login.Equals("dcbelmont")).First();
			var perfil = usuario.Perfis.Single(p => p.IdPerfil.Equals(5));
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			Assert.AreEqual(comparer.Compare(perfil.ToString(), "[UsuarioSistemaPerfil Usuario=DAVID COSTA BELMONT, Perfil=[SistemaPerfil CodigoSistema=SIGRH, CodigoPerfil=AUTENTIC  ]]"), 0);
		}
        
	}
}