using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class UsuarioCadastradoException : Exception
    {
        public UsuarioCadastradoException() { }
		
		public override string Message {
			get { return "Usuário já possui cadastro ativo."; }
		}
    }
}
