using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
	{
	    public UsuarioRepositorio(ISession session) : base(session){}
	
	}
}