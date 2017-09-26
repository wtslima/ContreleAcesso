using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	/// <summary>
	/// Mapeamento objeto-relacional da entidade 'Usuario'.
	/// </summary>
	public class UsuarioMapa : ControleMapa<Usuario>
	{
		/// <summary>
		/// Configuração do mapeamento da entidade 'Usuario'.
		/// </summary>
		public UsuarioMapa()
		{
            
			Table("TB_LOGIN");
			DynamicInsert();
			DynamicUpdate();


            Id(x => x.Id, "IDT_LOGIN").GeneratedBy.Identity();
            Map(x => x.Login, "CDA_LOGIN").Length(20).Not.Nullable();
			Map(x => x.Nome, "NOM_PESSOA_FISICA").Length(100).Not.Nullable();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();
			Map(x => x.IdPessoaFisica, "IDT_PESSOA_FISICA").Not.Nullable();

            HasMany(x => x.Perfis)
                .Table("TB_LOGIN_SISTEMA_PERFIL")
                .KeyColumn("IDT_LOGIN")
				.Inverse()
                .Fetch.Join().Cascade.SaveUpdate();
		}
	}
}