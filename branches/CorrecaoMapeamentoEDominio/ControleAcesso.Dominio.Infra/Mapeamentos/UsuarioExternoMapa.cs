using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Infra.Tipos;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class UsuarioExternoMapa : ControleMapa<UsuarioExterno>
	{
		public UsuarioExternoMapa()
		{
            
			Table("TB_LOGIN_EXTERNO");
			DynamicInsert();
			DynamicUpdate();
            Id(x => x.Id, "IDT_LOGIN_EXTERNO").GeneratedBy.Identity();
            Map(x => x.Login, "CDA_LOGIN_EXTERNO").CustomType<CaseInsensitiveStringType>().Length(20).Not.Nullable();
            Map(x => x.IdPessoaFisica, "IDT_PESSOA_FISICA").Not.Nullable().Unique();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();

            HasMany(x => x.Senhas)
                .Table("TB_LOGIN_EXTERNO_SENHA")
                .KeyColumn("IDT_LOGIN_EXTERNO")
                .Cascade.All()
                .Inverse()
                .Fetch.Join()
                .Cascade.SaveUpdate();

            HasMany(x => x.Perfis)
		        .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
                .KeyColumn("IDT_LOGIN_EXTERNO")
                .Cascade.All()
                .Inverse()
                .Fetch.Join()
                .Cascade.SaveUpdate();
		}
	}
}
