using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class LoginCadastradoException : Exception
    {
        private const string message = "Este login já é utilizado por outro usuário.";

        public LoginCadastradoException() : base(message) { }
        public LoginCadastradoException(Exception innerException) : base(message, innerException) { }
    }
}