using System.Linq;
using ControleAcesso.Dominio.Entidades;
using NUnit.Framework;

namespace ControleAcesso.Teste.Unitarios
{
	[TestFixture]
	public class SistemaTestes
	{
		[Test]
		public void AdicionarERemoverIPsSucesso() {
			var sistema = new Sistema {
				Codigo = "SERVIR",
                Nome = "Programa de excelência em serviços",
				Ativo = true
			};

            sistema.AdicionarIpServidorOrigem("ctinf-14rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 1);
			
			sistema.AdicionarIpServidorOrigem("rappdes01s.inmetro.local");
			sistema.AdicionarIpServidorOrigem("rapphom01s.inmetro.local");
            var retorno = sistema.AdicionarIpServidorOrigem("ctinf-14rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 3);

            sistema.RemoverIpServidorOrigem("ctinf-14rc.inmetro.local");
			Assert.AreEqual(sistema.ServidoresOrigem.Count(), 2);
		}
	}
}
