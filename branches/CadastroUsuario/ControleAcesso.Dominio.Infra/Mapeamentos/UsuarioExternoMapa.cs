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
            Map(x => x.IdPessoaFisica, "IDT_PESSOA_FISICA").Not.Nullable();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();
            Map(x => x.Nome).Formula("(select pf.NOM_PESSOA_FISICA from SIGRH.TB_PESSOA_FISICA pf where pf.IDT_PESSOA_FISICA = IDT_PESSOA_FISICA)");

            HasMany(x => x.Senhas)
                .Table("TB_LOGIN_EXTERNO_SENHA")
                .KeyColumn("CDA_LOGIN_EXTERNO")
                .Where("CDA_CONTROLE_ATIVO = 'A'")
                .Cascade.AllDeleteOrphan().Inverse();

            HasManyToMany<SistemaPerfil>(x => x.Perfis)
                .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
                .NotFound.Ignore()
                .ParentKeyColumn("CDA_LOGIN_EXTERNO")
                .ChildKeyColumns.Add("CDA_PERFIL", "CDA_SISTEMA");
		}
	}
}
