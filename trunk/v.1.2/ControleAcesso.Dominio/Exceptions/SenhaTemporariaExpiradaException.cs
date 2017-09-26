using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Exceptions
{
    public class SenhaTemporariaExpiradaException : Exception
    {
        private string _message;

        public SenhaTemporariaExpiradaException() { }
        public SenhaTemporariaExpiradaException(string message)
        {
            _message = message;
        }

        public override string Message {
            get {
                if (!string.IsNullOrWhiteSpace(_message))
                    return _message;

                return "A senha temporária está expirada.";
            }
        }
    }
}
