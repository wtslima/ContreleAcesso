using System;
using System.Collections.Generic;
using System.Linq;

using ControleAcesso.Dominio.Entidades;
using NHibernate;
using NHibernate.Linq;
using Corporativo.Utils.Linq;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class SistemaRepositorio : Repositorio<Sistema>
	{
		public SistemaRepositorio()
		{
			_ordenarPor = "Nome";
		}
		
		public override void Salvar(Sistema objeto)
		{
			base.SalvarComTransacao(objeto);
		}
	}
}
