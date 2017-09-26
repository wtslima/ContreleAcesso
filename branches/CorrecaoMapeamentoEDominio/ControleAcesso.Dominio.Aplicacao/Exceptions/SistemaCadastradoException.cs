using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class SistemaCadastradoException : Exception
    {

        private const string message = "Não foi possível cadastrar o sistema o Sistema.";
        public SistemaCadastradoException(): base(message) { }
    }
}