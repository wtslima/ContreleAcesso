using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class UsuarioSistemaPerfilMapa : ControleMapa<UsuarioSistemaPerfil>
	{
		public UsuarioSistemaPerfilMapa()
		{
            
			Table("TB_LOGIN_SISTEMA_PERFIL");
			DynamicInsert();
			DynamicUpdate();
			CompositeId()
                .KeyProperty(x => x.IdLogin, "IDT_LOGIN")
                .KeyProperty(x => x.IdPerfil, "IDT_PERFIL")
                .KeyProperty(x => x.IdSistema, "IDT_SISTEMA");

            References(x => x.Usuario, "IDT_LOGIN").Not.Insert().Not.Update();
			References(x => x.SistemaPerfil).Columns("IDT_PERFIL", "IDT_SISTEMA").Not.Insert().Not.Update();
		}
	}
}