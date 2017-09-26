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
				.KeyProperty(x => x.CodigoPerfil, "IDT_PERFIL")
				.KeyProperty(x => x.CodigoSistema, "IDT_SISTEMA");
			
			References(x => x.Sistema, "IDT_SISTEMA");
            References(x => x.Perfil, "IDT_PERFIL");

            HasManyToMany(x => x.UsuariosExternos).Inverse()
                .Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL")
                .ParentKeyColumns.Add("IDT_PERFIL", "IDT_SISTEMA")
                .ChildKeyColumn("IDT_LOGIN_EXTERNO");

		}
	}
}
