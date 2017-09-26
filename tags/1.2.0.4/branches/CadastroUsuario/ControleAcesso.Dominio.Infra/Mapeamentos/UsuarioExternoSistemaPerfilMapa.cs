using System;
using ControleAcesso.Dominio.Entidades;
using FluentNHibernate.Mapping;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{


    //public class UsuarioExternoSistemaPerfilMapa : ControleMapa<UsuarioExternoSistemaPerfil>
    //{
    //    public UsuarioExternoSistemaPerfilMapa()
    //    {
    //        Table("TB_LOGIN_EXTERNO_SISTEMA_PERFIL");
    //        DynamicInsert();
    //        DynamicUpdate();
    //        CompositeId()
    //            .KeyProperty(x => x.LoginUsuario, "CDA_LOGIN")
    //            .KeyReference(x => x.SistemaPerfil, "CDA_PERFIL", "CDA_SISTEMA");

    //        References(x => x.Usuario, "CDA_LOGIN").Not.Insert().Not.Update();
    //        Map(x => x.CodigoSistema, "CDA_SISTEMA");
    //        Map(x => x.CodigoPerfil, "CDA_PERFIL");
    //    }
    //}
}
