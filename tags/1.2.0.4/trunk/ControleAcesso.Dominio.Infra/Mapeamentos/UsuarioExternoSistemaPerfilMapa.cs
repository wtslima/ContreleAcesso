using System;
using ControleAcesso.Dominio.Entidades;
using FluentNHibernate.Mapping;

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
                .KeyProperty(x => x.LoginUsuario, "CDA_LOGIN_EXTERNO")
                .KeyProperty(x => x.CodigoPerfil, "CDA_PERFIL")
                .KeyProperty(x => x.CodigoSistema, "CDA_SISTEMA");

            References(x => x.Usuario, "CDA_LOGIN_EXTERNO").Not.Insert().Not.Update();
            References(x => x.SistemaPerfil).Columns("CDA_PERFIL", "CDA_SISTEMA").Not.Insert().Not.Update();
        }
    }
}