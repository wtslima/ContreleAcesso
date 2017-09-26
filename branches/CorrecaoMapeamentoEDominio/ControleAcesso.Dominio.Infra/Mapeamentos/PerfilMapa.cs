using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	/// <summary>
	/// Mapeamento objeto-relacional da entidade 'Perfil'.
	/// </summary>
	public class PerfilMapa : ControleMapa<Perfil>
	{
		/// <summary>
		/// Configuração do mapeamento da entidade 'Perfil'.
		/// </summary>
		public PerfilMapa()
		{
            
			Table("TB_PERFIL");
			Not.LazyLoad();
            Id(x => x.Id, "IDT_PERFIL").GeneratedBy.Identity();
			Map(x => x.Codigo, "CDA_PERFIL").Length(10).Not.Nullable();
			Map(x => x.Nome, "NOM_PERFIL").Length(20).Not.Nullable();
		}
	}
}
