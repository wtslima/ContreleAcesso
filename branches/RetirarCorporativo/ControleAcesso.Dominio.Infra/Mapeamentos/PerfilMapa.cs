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
			Id(x => x.Codigo, "CDA_PERFIL").GeneratedBy.Assigned();
			Map(x => x.Nome, "NOM_PERFIL").Length(20).Not.Nullable();
		}
	}
}
