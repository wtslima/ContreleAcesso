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
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        public ISession Session { get; set; }
        public Repositorio(ISession session)
        {
            Session = session;
        }

        protected string _ordenarPor;

        public string OrdenarPor
        {
            get { return _ordenarPor; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                _ordenarPor = value;
            }
        }
        public IEnumerable<T> Buscar(Expression<Func<T, bool>> criterio)
        {
            var retorno = Session.Query<T>().Where(criterio);
            return retorno;
        }

        public IEnumerable<T> Buscar()
        {
            var retorno = Session.Query<T>();
            return retorno;
        }

        public int TotalRegistros(Expression<Func<T, bool>> criterio)
        {
            return Session.Query<T>().Where(criterio).Count();
        }

        public void Salvar(T objeto)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Session.SaveOrUpdate(objeto);
                scope.Complete();
            }
            
        }

        public void Excluir(T objeto)
        {
            if (objeto is IControle)
            {
                (objeto as IControle).Excluido = true;
                Salvar(objeto);
            }
            else
            {
                Session.Delete(objeto);
            }
        }
        
    }
}