using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class SenhaTemporariaException : Exception
    {
        public override string Message
        {
            get
            {
                return "O usuário foi autenticado utilizando uma senha temporária.";
            }
        }
    }
}
