using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
    public class UsuarioExternoSistemaPerfilMapa : ControleMapa<UsuarioExternoSistemaPerfil>
    {
        public UsuarioExternoSistemaPerfilMapa()
        {
            
            Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL");
            DynamicInsert();
            DynamicUpdate();
            CompositeId()
                //.KeyProperty(x => x.Id, "IDT_LOGIN_EXTERNO")
                .KeyReference(x => x.Usuario, "IDT_LOGIN_EXTERNO")
                .KeyProperty(x => x.IdPerfil, "IDT_PERFIL")
                .KeyProperty(x => x.IdSistema, "IDT_SISTEMA");

            //References(x => x.Usuario, "IDT_LOGIN_EXTERNO").Not.Insert().Not.Update();
            References(x => x.SistemaPerfil).Columns("IDT_PERFIL", "IDT_SISTEMA").Not.Insert().Not.Update();
        }
    }
}