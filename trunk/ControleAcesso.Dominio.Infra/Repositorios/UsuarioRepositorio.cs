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
            //var session = this.Conexao.ObterSessao();
            //session.SaveOrUpdate(objeto);
            //session.Flush();

            var session = this.Conexao.ObterSessao(true);

            try
            {
                session.SaveOrUpdate(objeto);
                session.Flush();
            }
            catch
            {
                try
                {
                    session.SaveOrUpdateCopy(objeto);
                    session.Flush();
                }
                catch
                {
                    try
                    {
                        session.Merge(objeto);
                        session.Flush();
                    }
                    catch
                    {
                        session.Evict(objeto);
                        throw;
                    }
                }
            }

        }
	}
}