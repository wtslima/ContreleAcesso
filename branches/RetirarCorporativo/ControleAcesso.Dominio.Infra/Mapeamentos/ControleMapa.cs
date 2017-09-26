using System;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Infra.Tipos;
using FluentNHibernate.Mapping;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	/// <summary>
	/// Description of ControleMapa.
	/// </summary>
	public class ControleMapa<T> : ClassMap<T> where T : IControle
	{
		public ControleMapa()
		{
			Map(x => x.Ativo, "CDA_CONTROLE_ATIVO").CustomType(typeof(AtivacaoTipo)).Default("A");
            Map(x => x.Alteracao, "DAT_CONTROLE_ALTERACAO").Not.Nullable().Default("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'");
            Map(x => x.Origem, "CDA_CONTROLE_ORIGEM").Default("I");
            Map(x => x.Uso, "CDA_CONTROLE_USO").Default("I");
		}
	}
}
