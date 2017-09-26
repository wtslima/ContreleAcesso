using System;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class PerfilRepositorio : Repositorio<Perfil>
	{
		public PerfilRepositorio()
		{
			_ordenarPor = "Nome";
		}
	}
}
