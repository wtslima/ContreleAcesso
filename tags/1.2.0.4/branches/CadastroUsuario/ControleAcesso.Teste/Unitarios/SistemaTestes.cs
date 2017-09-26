using System;
using System.Linq;
using ControleAcesso.Dominio.Entidades;
using NUnit.Framework;

namespace ControleAcesso.Teste.Unitarios
{
	[TestFixture]
	public class SistemaTestes
	{
		[Test]
		public void AdicionarERemoverIPs_Sucesso() {
			var sistema = new Sistema {
				Codigo = "SIGRH",
				Nome = "Sistema Integrado de Gestão de Recursos Humanos",
				Ativo = true
			};
			
			sistema.AdicionarIpServidorOrigem("sesis-40rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 1);
			
			sistema.AdicionarIpServidorOrigem("rappdes01s.inmetro.local");
			sistema.AdicionarIpServidorOrigem("rapphom01s.inmetro.local");
			var retorno = sistema.AdicionarIpServidorOrigem("sesis-40rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 3);
			
			sistema.RemoverIpServidorOrigem("sesis-40rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 2);
		}
	}
}
