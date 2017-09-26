using System.Transactions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class SistemaRepositorio : Repositorio<Sistema>, ISistemaRepositorio
	{
	    public SistemaRepositorio(ISession session) : base(session){}

	    public  Sistema Cadastrar(Sistema sistema)
	    {
	        using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.Save(sistema);
                scope.Complete();
            }
	        return sistema;
	    }

	    public Sistema Atualizar(Sistema sistema)
	    {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.Update(sistema);
                scope.Complete();
            }
            return sistema;
	    }
	}
}
