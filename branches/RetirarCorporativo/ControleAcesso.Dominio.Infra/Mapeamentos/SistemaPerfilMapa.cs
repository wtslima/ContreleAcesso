using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class SistemaPerfilMapa : ControleMapa<SistemaPerfil>
	{
		public SistemaPerfilMapa()
		{
            
			Table("TB_SISTEMA_PERFIL");
            DynamicInsert();
            DynamicUpdate();
			CompositeId()
				.KeyProperty(x => x.CodigoPerfil, "CDA_PERFIL")
				.KeyProperty(x => x.CodigoSistema, "CDA_SISTEMA");
			
			References(x => x.Sistema, "CDA_SISTEMA");
            References(x => x.Perfil, "CDA_PERFIL");

            HasManyToMany(x => x.UsuariosExternos).Inverse()
                .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
                .ParentKeyColumns.Add("CDA_PERFIL", "CDA_SISTEMA")
                .ChildKeyColumn("CDA_LOGIN_EXTERNO");

		}
	}
}
