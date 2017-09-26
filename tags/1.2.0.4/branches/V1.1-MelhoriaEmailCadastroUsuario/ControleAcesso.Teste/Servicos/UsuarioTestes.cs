using System;
using System.Linq;
using System.Threading;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class UsuarioTestes
	{
		public UsuarioTestes() {
			log4net.Config.XmlConfigurator.Configure();
		}
		
		#region AutenticarUsuario
        [TestCase("SIGRH", "fulaninho", "123", ExpectedException = typeof(LoginInexistenteException))]
        [TestCase("SIGRH", "dcbelmont", "123", ExpectedException = typeof(SenhaInvalidaException))]
        [TestCase("SIGRH", "svnappadmin", "inmetro@0909", ExpectedException = typeof(LoginNaoAssociadoAPessoaException))]
        /*TODO: Acrescentar caso de teste para login válido
 		* [TestCase("SIGRH", "dcbelmont", "xpto")]*/
 		//TODO: Acrescentar caso de teste para usuário sem perfil de acesso a sistema
        public void Autenticar(string sistema, string usuario, string senha) {
            UsuarioServico.Instancia.AutenticarUsuario(sistema, usuario, senha);
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