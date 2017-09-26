using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleAcessoService.DataContracts;

namespace ControleAcesso.Teste.Elemento
{
    public class UsuarioRequisicao
    {
        public class AutenticacaoRequisicao
        {
            public string token { get; set; }
            public Login login { get; set; }
        }

        public class FiltroUsuarioRequisicao
        {
            public string token { get; set; }
            public int filtro { get; set; }
            public bool filtrarPorSistema { get; set; }
        }

        public class NovoUsuarioRequisicao
        {
            public string token { get; set; }

            public Usuario usuario { get; set; }
        }
    }
}
