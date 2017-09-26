using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class LoginInexistenteException : Exception
	{
		private string _loginUsuario;

        public string Login { get { return _loginUsuario; } }

		public LoginInexistenteException(string loginUsuario) : base() {
			_loginUsuario = loginUsuario;
		}

		public LoginInexistenteException(string loginUsuario, string message) : base(message) {
			_loginUsuario = loginUsuario;
		}
		
		public override string Message {
			get { return string.Format("O login '{0}' não existe.", _loginUsuario); }
		}
	}
}