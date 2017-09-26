using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Exceptions
{
  public class NaoAutorizadoException : Exception
    {
        private string _message;

        public NaoAutorizadoException() { }
        public NaoAutorizadoException(string message)
        {
            _message = message;
        }

        public override string Message {
            get {
                if (!string.IsNullOrWhiteSpace(_message))
                    return _message;

                return "Não autorizado.";
            }
        }
    }
}

