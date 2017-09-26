using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ControleAcesso.Dominio.Interfaces.Repositorio
{
    public interface IRepositorio<T> where T : class
    {
        string OrdenarPor { get; set; }
        int TotalRegistros(Expression<Func<T, bool>> criterio);
        IEnumerable<T> Buscar(Expression<Func<T, bool>> criterio);
        IEnumerable<T> Buscar(); 
        void Salvar(T objeto);
        void Excluir(T objeto );
    }
}