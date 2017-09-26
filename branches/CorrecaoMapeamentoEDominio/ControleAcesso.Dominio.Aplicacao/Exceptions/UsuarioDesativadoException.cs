using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class UsuarioDesativadoException : Exception
    {
        public UsuarioDesativadoException() {}
		
		public override string Message {
			get { return "Usuário esta desativado."; }
		}
    }
}
