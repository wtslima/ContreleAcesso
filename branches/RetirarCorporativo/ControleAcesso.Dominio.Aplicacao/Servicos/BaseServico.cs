using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Interfaces.Repositorio;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    public class BaseServico<T> : IServicoApp<T> where T : class
    {
        private readonly IRepositorio<T> _repositorio;

        public BaseServico(IRepositorio<T> repositorio)
        {
            _repositorio = repositorio;
        }
        protected IRepositorio<T> Repositorio
        {
            get { return _repositorio; }
        }
        public string OrdenarPor { get; set; }
        public IEnumerable<T> Buscar(Expression<Func<T, bool>> criterio)
        {
            var retorno = _repositorio.Buscar(criterio);
            return retorno;
        }

        public IEnumerable<T> Buscar()
        {
            var retorno = _repositorio.Buscar();
            return retorno;
        }
        public int TotalRegistros(Expression<Func<T, bool>> criterio)
        {
            return _repositorio.TotalRegistros(criterio);
        }
        public void Salvar(T objeto)
        {
             _repositorio.Salvar(objeto);
        }
        public void Excluir(T objeto)
        {
            _repositorio.Excluir(objeto);
        }
    }
}