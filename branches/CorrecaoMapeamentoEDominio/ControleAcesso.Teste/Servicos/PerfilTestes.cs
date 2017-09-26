using System;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using log4net.Config;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class PerfilTestes
	{
	    private BaseServico<Perfil> _servico; 
		public PerfilTestes(BaseServico<Perfil> servico)
		{
		    _servico = servico;
		    XmlConfigurator.Configure();
		}

	    [Test]
		public void BuscaPerfisSucesso() {
            var perfis = _servico.Buscar(p => p.Excluido == false);
			Assert.GreaterOrEqual(perfis.Count(), 1);
		}
		
		[Test]
		public void BuscaPerfilSucesso() {
            var perfil = _servico.Buscar(p => p.Codigo.ToUpper().Trim().Equals("AUTENTIC")).FirstOrDefault();
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			Assert.AreEqual(comparer.Compare(perfil.ToString(), "[Perfil Codigo=AUTENTIC  , Nome=Usuário Autenticado com acesso básico]"), 0);
		}
	}
}