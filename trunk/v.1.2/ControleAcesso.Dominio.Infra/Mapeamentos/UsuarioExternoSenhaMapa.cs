﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
    public class UsuarioExternoSenhaMapa : ControleMapa<UsuarioExternoSenha>
    {
        public UsuarioExternoSenhaMapa()
        {
            Table("TB_LOGIN_EXTERNO_SENHA");
            DynamicInsert();
            DynamicUpdate();
            CompositeId()
                .KeyProperty(x => x.Login, "CDA_LOGIN_EXTERNO")
                .KeyProperty(x => x.Tipo, "CDA_TIPO_SENHA");
            Map(x => x.Valor, "DES_SENHA").Length(100).Not.Nullable();
            Map(x => x.Expiracao, "DAT_EXPIRACAO_SENHA").Not.Nullable();
            References(x => x.UsuarioExterno, "CDA_LOGIN_EXTERNO").Not.Insert().Not.Update();
        }
    }
}
