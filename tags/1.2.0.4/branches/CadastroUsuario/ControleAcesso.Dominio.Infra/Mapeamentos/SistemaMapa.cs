using System;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	/// <summary>
	/// Mapeamento objeto-relacional da entidade 'Sistema'.
	/// </summary>
	public class SistemaMapa : ControleMapa<Sistema>
	{
		/// <summary>
		/// Configuração do mapeamento da entidade 'Sistema'.
		/// </summary>
		public SistemaMapa()
		{
			Table("TB_SISTEMA");
			DynamicInsert();
			DynamicUpdate();
			Id(x => x.Codigo, "CDA_SISTEMA").GeneratedBy.Assigned().Length(20);
			Map(x => x.Nome, "NOM_SISTEMA").Length(100);
			HasMany(x => x.ServidoresOrigem)
				.EntityName("ControleAcesso.Dominio.ObjetosDeValor.ServidorOrigem")
				.Table("TS_PARAMETRO")
				.KeyColumn("NOM_PARAMETRO")
				.Cascade.All()
				.Inverse();
			HasManyToMany(x => x.PerfisAcesso)
				.Table("TB_SISTEMA_PERFIL")
				.ParentKeyColumn("CDA_SISTEMA")
				.ChildKeyColumn("CDA_PERFIL");
		}
	}
}
