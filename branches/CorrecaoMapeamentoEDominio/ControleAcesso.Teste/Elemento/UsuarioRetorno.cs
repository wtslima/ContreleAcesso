using System.Collections.Generic;
using ControleAcessoService.DataContracts;

namespace ControleAcesso.Teste.Elemento
{
    public class UsuarioRetorno
    {
        public class TodosUsuariosRetorno
        {
            public List<Usuario> TodosUsuariosResult { get; set; }
        }

        public class AutenticarUsuarioExternoRetorno
        {
            public Usuario AutenticarUsuarioExternoResult { get; set; }
        }

        public class ObterTodosUsuariosRetorno
        {
            public List<Usuario> ObterTodosUsuariosResult { get; set; }
        }

        public class NovoUsuarioRetorno
        {
            public Usuario NovoUsuarioResult { get; set; }
        }
    }
}
