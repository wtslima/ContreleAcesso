﻿using System;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using NHibernate.Util;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
	[TestFixture]
	public class PerfilTestes
	{
		public PerfilTestes() {
			log4net.Config.XmlConfigurator.Configure();
		}
		
		[Test]
		public void BuscaPerfis_Sucesso() {
			var servico =  Servico<Perfil>.NovaInstancia;
			var perfis = servico.Buscar(p => p.Ativo == true);
			Assert.GreaterOrEqual(perfis.Count(), 1);
		}
		
		[Test]
		public void BuscaPerfil_Sucesso() {
            var perfil = Servico<Perfil>.Instancia.Buscar(p => p.Codigo.ToUpper().Trim().Equals("AUTENTIC")).FirstOrDefault();
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			Assert.AreEqual(comparer.Compare(perfil.ToString(), "[Perfil Codigo=AUTENTIC  , Nome=Usuário Autenticado com acesso básico]"), 0);
		}
	}
}