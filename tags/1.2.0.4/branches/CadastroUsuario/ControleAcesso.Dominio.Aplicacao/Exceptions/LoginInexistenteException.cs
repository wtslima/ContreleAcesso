using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class LoginInexistenteException : Exception
	{
		private string _loginUsuario;
		
		public LoginInexistenteException(string loginUsuario) {
			_loginUsuario = loginUsuario;
		}
		
		public override string Message {
			get { return string.Format("O login '{0}' não existe.", _loginUsuario); }
		}
	}
}