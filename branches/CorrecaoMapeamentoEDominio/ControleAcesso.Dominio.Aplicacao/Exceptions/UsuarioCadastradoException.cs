using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class UsuarioCadastradoException : Exception
    {
        private const string message = "Usuário já possui cadastro ativo.";

        public UsuarioCadastradoException() : base(message) { }
        public UsuarioCadastradoException(Exception innerException) : base(message, innerException) { }
    }
}