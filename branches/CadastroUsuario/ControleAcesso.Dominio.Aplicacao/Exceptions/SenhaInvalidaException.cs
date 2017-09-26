using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class SenhaInvalidaException : Exception
	{
		public override string Message {
			get { return "A senha informada não é válida."; }
		}
	}
}