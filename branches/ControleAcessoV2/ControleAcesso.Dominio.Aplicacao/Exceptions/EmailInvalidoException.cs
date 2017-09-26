using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
    public class EmailInvalidoException : Exception
    {
        public override string Message
        {
            get { return "Email inválido"; }
        }
    }
}
