using System;
using ControleAcesso.Dominio.Entidades;
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
            Map(x => x.Excluido, "BLN_CONTROLE_EXCLUIDO").Default("0").Not.Nullable();
            Map(x => x.Alteracao, "DAT_CONTROLE_ALTERACAO").Not.Nullable().Default("'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'");
            Map(x => x.Origem, "CDA_CONTROLE_ORIGEM").Default("I").Not.Nullable();
		}
	}
}
