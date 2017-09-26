using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using log4net.Config;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class SistemaTestes
	{
	    private SistemaServico _servico;
	    private BaseServico<Perfil> _servicoPerfil; 
		public SistemaTestes(SistemaServico servico, BaseServico<Perfil> servicoPerfil)
		{
		    _servico = servico;
		    _servicoPerfil = servicoPerfil;
		    XmlConfigurator.Configure();
		}

	    [Test]
		public void AdicionarEnderecoIpServidorOrigemSucesso() {
			var sistema = _servico.Buscar(s => s.Codigo == "SIGRH").FirstOrDefault();
			sistema.AdicionarIpServidorOrigem("rappdes01s.inmetro.local");
			
			_servico.Salvar(sistema);
		}
		
		[Test]
		public void AdicionarNovoSistemaEIpServidorOrigemSucesso() {
			var sistema = _servico.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).SingleOrDefault();
			if (sistema != null) {
				sistema.RemoverPerfilAcesso(5);
			}
			
			_servico.Excluir(1);
			
			sistema = new Sistema{
				Codigo = "PONTOFOCAL",
				Nome = "Ponto Focal - Alerta Exportador"
			};
			
			sistema.AdicionarIpServidorOrigem("sesis-40rc.inmetro.local");
			_servico.Salvar(sistema);
		}
		
		[Test]
		public void BuscarIpServidorOrigemSucesso() {
			var sistema = _servico.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).SingleOrDefault();
			Assert.IsTrue(sistema.ServidoresOrigem.Any(i => i.Servidor.ToLower().Equals("sesis-40rc.inmetro.local")));
		}
		
		[Test]
		public void AtribuirPerfilSistemaSucesso() {
			//TODO: Não está efetivamente inserindo qualquer registro na TB_SISTEMA_PERFIL.
			var sistema = _servico.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).Single();
            var perfil = _servicoPerfil.Buscar(p => p.Codigo.Trim().Equals("AUTENTIC")).First();
			sistema.AdicionarPerfilAcesso(perfil);
			_servico.Salvar(sistema);
		}
		
		#region Validação de token
		[Test]
		public void ValidarToken_Sucesso() {
			Assert.IsTrue(_servico.ValidarToken("9168cc74d8be8fb3bc155dc2ff3fd332"));
		}
		
		[Test]
		public void ValidarToken_Falha_TokenInvalido() {
			try {
				_servico.ValidarToken("9168cc74d8be8fb3bc155dc2ff3fd332");
				Assert.Fail("Era esperado que fosse lançada uma exceção do tipo 'TokenInvalidoException'.");
			} catch (TokenInvalidoException ex) {
				Assert.AreEqual("O token informado 9168cc74d8be8fb3bc155dc2ff3fd332 é inválido.", ex.Message);
			}
		}
		
		[Test]
		[ExpectedException(typeof(TokenInvalidoException))]
		public void ValidarToken_Falha_TokenValidoComIPInvalido() {
			_servico.ValidarToken("8827fa9e29730dd539a51a2a71004a45");
		}
		#endregion
		
		[Test]
		public void BuscaPerfisSistemaSucesso() {
			var servico = _servico;
			var sistema = servico.Buscar(s => s.Codigo.Equals("SIGRH")).FirstOrDefault();
			var perfis = sistema.PerfisAcesso;
			Assert.IsTrue(perfis.Any(p => p.Codigo.Trim().Equals("AUTENTIC")));
		}
	}
}
