using System;
using System.Collections.Generic;
using ControleAcesso.Dominio.Infra.Tipos;
using ControleAcesso.Dominio.Entidades;
using FluentNHibernate;
using FluentNHibernate.Conventions.Inspections;
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
			Id(x => x.Login, "CDA_LOGIN_EXTERNO").GeneratedBy.Assigned().CustomType<CaseInsensitiveStringType>().Length(20).Not.Nullable();
            Map(x => x.IdPessoaFisica, "IDT_PESSOA_FISICA").Not.Nullable().Unique();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();

		    HasMany(x => x.Senhas)
		        .Table("TB_LOGIN_EXTERNO_SENHA")
		        .KeyColumn("CDA_LOGIN_EXTERNO")
		        .Where("CDA_CONTROLE_ATIVO = 'A'")
		        .Inverse()
		        .Fetch.Join().Cascade.SaveUpdate();

            HasMany(x => x.Perfis)
		        .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
		        .KeyColumn("CDA_LOGIN_EXTERNO")
                .Inverse()
                .Fetch.Join().Cascade.SaveUpdate();
		}
	}
}
