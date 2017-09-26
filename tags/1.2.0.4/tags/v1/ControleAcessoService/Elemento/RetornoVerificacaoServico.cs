using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleAcessoService.Elemento
{
    public class RetornoVerificacaoServico
    {
        public string ScHash { get; set; }
        public string Mensagem { get; set; }
        public string EHash { get; set; }
        public bool Consulta { get; set; }
    }
}