using System;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class SistemaPerfilMapa : ControleMapa<SistemaPerfil>
	{
		public SistemaPerfilMapa()
		{
			Table("TB_SISTEMA_PERFIL");
			CompositeId()
				.KeyProperty(x => x.CodigoPerfil, "CDA_PERFIL")
				.KeyProperty(x => x.CodigoSistema, "CDA_SISTEMA");
			
			References(x => x.Sistema, "CDA_SISTEMA").Not.Insert().Not.Update();
            References(x => x.Perfil, "CDA_PERFIL").Not.Insert().Not.Update();



            HasManyToMany<UsuarioExterno>(x => x.UsuariosExternos).Inverse()
                .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
                .ParentKeyColumns.Add("CDA_PERFIL", "CDA_SISTEMA")
                .ChildKeyColumn("CDA_LOGIN_EXTERNO");

		}
	}
}
