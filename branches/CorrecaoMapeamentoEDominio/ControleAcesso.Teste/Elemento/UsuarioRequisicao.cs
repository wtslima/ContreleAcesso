using ControleAcessoService.DataContracts;

namespace ControleAcesso.Teste.Elemento
{
    public class UsuarioRequisicao
    {
        public class AutenticacaoRequisicao
        {
            public string Token { get; set; }
            public Login Login { get; set; }
        }

        public class FiltroUsuarioRequisicao
        {
            public string Token { get; set; }
            public int Filtro { get; set; }
            public bool FiltrarPorSistema { get; set; }
        }

        public class NovoUsuarioRequisicao
        {
            public string Token { get; set; }

            public Usuario Usuario { get; set; }
        }
    }
}
