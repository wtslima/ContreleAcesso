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
			Id(x => x.Codigo, "CDA_SISTEMA").GeneratedBy.Assigned().Length(20);
			Map(x => x.Nome, "NOM_SISTEMA").Length(100);
		    HasMany(x => x.ServidoresOrigem)
		        .Table("TS_PARAMETRO")
		        .KeyColumn("NOM_PARAMETRO")
		        .Cascade.All()
				.Inverse();
		    HasMany(x => x.PerfisAcesso)
		        .Table("TB_SISTEMA_PERFIL")
		        .KeyColumn("CDA_SISTEMA")
		        .KeyColumn("CDA_PERFIL");



		}
	}
}
