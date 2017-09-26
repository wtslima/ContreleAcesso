using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class SistemaRepositorio : Repositorio<Sistema>, ISistemaRepositorio
	{
	    public SistemaRepositorio(ISession session) : base(session){}
	}
}
