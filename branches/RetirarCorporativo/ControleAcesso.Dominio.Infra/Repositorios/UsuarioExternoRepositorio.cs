using System.Transactions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;



namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class UsuarioExternoRepositorio : Repositorio<UsuarioExterno>, IUsuarioExternoRepositorio
    {
       

        public UsuarioExternoRepositorio(ISession session)
            : base(session)
        {
            Session = session;
        }
        public virtual void Cadastrar(UsuarioExterno objeto)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.SaveOrUpdate(objeto);
                scope.Complete();
            }
        }


       
    }
}