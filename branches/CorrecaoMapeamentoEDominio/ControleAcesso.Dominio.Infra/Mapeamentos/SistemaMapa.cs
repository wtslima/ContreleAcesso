using ControleAcesso.Dominio.Entidades;
using FluentNHibernate.Conventions.Inspections;

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
            Id(x => x.Id, "IDT_SISTEMA").GeneratedBy.Identity();
			Map(x => x.Codigo, "CDA_SISTEMA").Not.Nullable().Length(20);
			Map(x => x.Nome, "NOM_SISTEMA").Length(100);
		    HasMany(x => x.ServidoresOrigem)
		        .Table("TS_PARAMETRO")
                .KeyColumn("IDT_SISTEMA")
		        .Cascade.All()
				.Inverse();
		    HasMany(x => x.PerfisAcesso)
		        .Table("TB_SISTEMA_PERFIL")
		        .KeyColumn("IDT_SISTEMA")
		        .KeyColumn("IDT_PERFIL")
                .Cascade.All()
                .Inverse();



		}
	}
}
