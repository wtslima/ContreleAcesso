using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface IServicoApp<T> where T : class
    {
        string OrdenarPor { get; set; }
        IEnumerable<T> Buscar(Expression<Func<T, bool>> criterio);
        IEnumerable<T> Buscar(); 
        int TotalRegistros(Expression<Func<T, bool>> criterio);
        void Salvar(T objeto);
        void Excluir(T objeto);
    }
}