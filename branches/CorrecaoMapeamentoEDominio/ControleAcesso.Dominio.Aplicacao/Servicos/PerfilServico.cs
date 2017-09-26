using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    public class PerfilServico : BaseServico<Perfil>, IPerfilServicoApp
    {
        private readonly IPerfilRepositorio _repositorio;

        public PerfilServico(IPerfilRepositorio repositorio)
            : base(repositorio)
        {
            _repositorio = repositorio;
        }
       
        public IEnumerable<Perfil> BuscarPerfisPorCodigo(string[] codigo)
        {
          return _repositorio.BuscarPerfisPorCodigo(codigo);
        }

        public IEnumerable<Perfil> BuscarPorCodigo(Expression<Func<Perfil, bool>> criterio)
        {
            return Buscar(c => c.Codigo.Equals(criterio));
        }

        public Perfil Cadastrar(Perfil objeto)
        {
            try
            {
                if (objeto != null)
                {
                    var novoPerfil = new Perfil()
                    {
                        Codigo = objeto.Codigo,
                        Nome = objeto.Nome,
                        Descricao = objeto.Descricao
                    };
                   
                    return _repositorio.Salvar(novoPerfil);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível cadastrar o perfil", ex);
            }
            //TODO:
            throw null;
        }

        public Perfil Atualizar(Perfil objeto)
        {
            try
            {
                if (objeto.Id > 0)
                {
                    return _repositorio.Atualizar(objeto);
                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public void Excluir(int id)
        {
            var perfil = Buscar(s => s.Id == id).FirstOrDefault();
            Excluir(perfil);
        }

       
    }
}