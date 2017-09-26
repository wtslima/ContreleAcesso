using System.Transactions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;
using System.Collections.Generic;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class SistemaPerfilRepositorio : Repositorio<SistemaPerfil>, ISistemaPerfilRepositorio
	{
          public SistemaPerfilRepositorio(ISession session) : base(session){}
        
          public void AssociarPerfilSistema(List<SistemaPerfil> lstSisPerfil)
          {
              using (var scope = new TransactionScope(TransactionScopeOption.Required))
              {
                  foreach (var sistemaperfil in lstSisPerfil)
                  {
                      Session.Save(sistemaperfil);
                      scope.Complete();
                  }



                  
                  
              }
          }


    }
}
