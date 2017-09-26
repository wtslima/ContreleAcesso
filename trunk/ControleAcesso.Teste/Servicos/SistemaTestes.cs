using System;
using System.Collections.Generic;
using System.Linq;

using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.ObjetosDeValor;
using NHibernate.Util;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class SistemaTestes
	{
		public SistemaTestes() {
			log4net.Config.XmlConfigurator.Configure();
		}
		
		[Test]
		public void AdicionarEnderecoIpServidorOrigem_Sucesso() {
			var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo == "SIGRH").FirstOrDefault();
			sistema.AdicionarIpServidorOrigem("rappdes01s.inmetro.local");
			
			SistemaServico.Instancia.Salvar(sistema);
		}
		
		[Test]
		public void AdicionarNovoSistemaEIpServidorOrigem_Sucesso() {
			var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).SingleOrDefault();
			if (sistema != null) {
				sistema.RemoverPerfilAcesso("AUTENTIC");
			}
			
			SistemaServico.Instancia.Excluir("PONTOFOCAL");
			
			sistema = new Sistema{
				Codigo = "PONTOFOCAL",
				Nome = "Ponto Focal - Alerta Exportador"
			};
			
			sistema.AdicionarIpServidorOrigem("sesis-40rc.inmetro.local");
			SistemaServico.NovaInstancia.SalvarComTransacao(sistema);
		}
		
		[Test]
		public void BuscarIpServidorOrigem_Sucesso() {
			var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).SingleOrDefault();
			Assert.IsTrue(sistema.ServidoresOrigem.Any(i => i.Servidor.ToLower().Equals("sesis-40rc.inmetro.local")));
		}
		
		[Test]
		public void AtribuirPerfilSistema_Sucesso() {
			//TODO: Não está efetivamente inserindo qualquer registro na TB_SISTEMA_PERFIL.
			var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals("PONTOFOCAL")).Single();
			var perfil = Servico<Perfil>.Instancia.Buscar(p => p.Codigo.Trim().Equals("AUTENTIC")).First();
			sistema.AdicionarPerfilAcesso(perfil);
			SistemaServico.Instancia.SalvarComTransacao(sistema);
		}
		
		#region Validação de token
		[Test]
		public void ValidarToken_Sucesso() {
			Assert.IsTrue(SistemaServico.Instancia.ValidarToken("9168cc74d8be8fb3bc155dc2ff3fd332"));
		}
		
		[Test]
		public void ValidarToken_Falha_TokenInvalido() {
			try {
				SistemaServico.Instancia.ValidarToken("9168cc74d8be8fb3bc155dc2ff3fd332");
				Assert.Fail("Era esperado que fosse lançada uma exceção do tipo 'TokenInvalidoException'.");
			} catch (TokenInvalidoException ex) {
				Assert.AreEqual("O token informado 9168cc74d8be8fb3bc155dc2ff3fd332 é inválido.", ex.Message);
			}
		}
		
		[Test]
		[ExpectedException(typeof(TokenInvalidoException))]
		public void ValidarToken_Falha_TokenValidoComIPInvalido() {
			SistemaServico.Instancia.ValidarToken("8827fa9e29730dd539a51a2a71004a45");
		}
		#endregion
		
		[Test]
		public void BuscaPerfisSistema_Sucesso() {
			var servico = SistemaServico.Instancia;
			var sistema = servico.Buscar(s => s.Codigo.Equals("SIGRH")).FirstOrDefault();
			var perfis = sistema.PerfisAcesso;
			Assert.IsTrue(perfis.Any(p => p.Codigo.Trim().Equals("AUTENTIC")));
		}
	}
}
