using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CONTROLE_DE_ACESSO_WEB.Models.Sistemas
{
    public class FiltroListarSistemasModel
    { 
        public int Id { get; set; }
        public string CodSistema { get; set; }
        public string DescSistema { get; set; }
    }
}