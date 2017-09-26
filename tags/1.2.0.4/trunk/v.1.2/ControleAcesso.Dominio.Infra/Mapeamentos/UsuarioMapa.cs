﻿using ControleAcesso.Dominio.Entidades;

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
			Id(x => x.Login, "CDA_LOGIN").GeneratedBy.Assigned().Length(20).Not.Nullable();
			Map(x => x.Nome, "NOM_PESSOA_FISICA").Length(100).Not.Nullable();
			Map(x => x.Email, "DES_EMAIL").Length(100).Nullable();
			Map(x => x.CPF, "CDA_CPF").Length(11).Not.Nullable();
		    References(x => x.PessoaFisica).Formula("(select pf.IDT_PESSOA_FISICA from SIGRH.TB_PESSOA_FISICA as pf where pf.CDA_CPF = CDA_CPF)").Not.Insert().Not.Update();
            
            HasMany(x => x.Perfis)
                .Table("TB_LOGIN_SISTEMA_PERFIL")
                .KeyColumn("CDA_LOGIN")
                .Inverse();
		}
	}
}