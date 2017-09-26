using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class TokenInvalidoException : Exception
	{
		private string _token;
		
		public TokenInvalidoException(string token, Exception innerException = null) : base("", innerException)
		{
			_token = token;
		}
		
		public override string Message {
			get { return string.Format("O token informado {0} é inválido.", _token); }
		}
	}
}