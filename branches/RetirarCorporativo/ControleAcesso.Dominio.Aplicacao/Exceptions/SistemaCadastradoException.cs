using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class SistemaCadastradoException : Exception
    {

        private const string message = "Este Sistema já foi cadastrado.";

        public SistemaCadastradoException(): base(message) { }
        public SistemaCadastradoException(Exception innException) : base(message, innException) { }
    }
}