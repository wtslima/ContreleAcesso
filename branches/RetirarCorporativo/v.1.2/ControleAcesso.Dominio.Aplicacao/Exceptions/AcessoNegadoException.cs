using System;

namespace ControleAcesso.Dominio.Aplicacao.Exceptions
{
	public class AcessoNegadoException : Exception
	{
		public AcessoNegadoException() {}
		
		public override string Message {
			get { return "Acesso negado a este sistema."; }
		}
	}
}
