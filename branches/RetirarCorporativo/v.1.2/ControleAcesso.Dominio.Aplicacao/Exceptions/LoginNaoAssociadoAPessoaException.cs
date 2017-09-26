using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class LoginNaoAssociadoAPessoaException : Exception
	{
		private string _loginUsuario;
		
		public LoginNaoAssociadoAPessoaException(string loginUsuario) {
			_loginUsuario = loginUsuario;
		}
		
		public override string Message {
			get { return string.Format("O login '{0}' existe na rede mas ainda não foi associado a uma pessoa física.", _loginUsuario); }
		}
	}
}
