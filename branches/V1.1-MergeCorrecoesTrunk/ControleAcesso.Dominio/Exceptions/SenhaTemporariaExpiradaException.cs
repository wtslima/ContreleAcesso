using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Exceptions
{
    public class SenhaTemporariaExpiradaException : Exception
    {
        private string _message;

        public string NovaSenha { get; private set; }

        public SenhaTemporariaExpiradaException() { }
        public SenhaTemporariaExpiradaException(string message, string senha)
        {
            _message = message;
            NovaSenha = senha;
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
