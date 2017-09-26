using System;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class UsuarioRepositorio : Repositorio<Usuario>
	{
		public UsuarioRepositorio()
		{
			_ordenarPor = "Nome";
		}
		
		public override void Salvar(Usuario objeto)
		{
			base.SalvarComTransacao(objeto);
		}
	}
}