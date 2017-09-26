using System;
using System.Linq;
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
            var session = this.Conexao.ObterSessao();
            session.Persist(objeto);
            objeto.Perfis.ToList().ForEach(session.Persist);
            session.SaveOrUpdate((object)objeto);
            session.Flush();
        }
	}
}