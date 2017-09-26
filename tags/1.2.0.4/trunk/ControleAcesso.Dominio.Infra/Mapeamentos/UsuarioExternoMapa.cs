using System;
using ControleAcesso.Dominio.Entidades;
using FluentNHibernate.Mapping;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class UsuarioExternoMapa : ControleMapa<UsuarioExterno>
	{
		public UsuarioExternoMapa()
		{
			Table("TB_LOGIN_EXTERNO");
			DynamicInsert();
			DynamicUpdate();
			Id(x => x.Login, "CDA_LOGIN_EXTERNO").GeneratedBy.Assigned().Length(20).Not.Nullable();
            Map(x => x.IdPessoaFisica, "IDT_PESSOA_FISICA").Not.Nullable().Unique();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();

            HasMany(x => x.Senhas)
                .Table("TB_LOGIN_EXTERNO_SENHA")
                .KeyColumn("CDA_LOGIN_EXTERNO")
                .Where("CDA_CONTROLE_ATIVO = 'A'")
                .Inverse();

		    HasMany(x => x.Perfis)
		        .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
		        .KeyColumn("CDA_LOGIN_EXTERNO")
                .Inverse();
		}
	}
}
