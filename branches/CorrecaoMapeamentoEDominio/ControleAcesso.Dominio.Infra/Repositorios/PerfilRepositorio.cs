using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NHibernate;
using NHibernate.Linq;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
    public class PerfilRepositorio : Repositorio<Perfil>, IPerfilRepositorio
    {
        public PerfilRepositorio(ISession session)
            : base(session)
        {
            Session = session;
        }

        public IEnumerable<Perfil> BuscarPerfisPorCodigo(string[] codigos)
        {
            var resultado = Session.Query<Perfil>().SelectMany(p => p.Codigo.Where(s => s.Equals(codigos)));
            return resultado as IEnumerable<Perfil>;
        }

        public IEnumerable<Perfil> BuscarPorCodigo(Expression<Func<Perfil, bool>> criterio)
        {
            var retorno = Session.Query<Perfil>().Where(criterio);
            return retorno;
        }

        public Perfil Salvar(Perfil perfil)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.Save(perfil);
                scope.Complete();
            }
            return perfil;
        }

        public Perfil Atualizar(Perfil objeto)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.Update(objeto);
                scope.Complete();
            }
            return objeto;
        }
    }
}
