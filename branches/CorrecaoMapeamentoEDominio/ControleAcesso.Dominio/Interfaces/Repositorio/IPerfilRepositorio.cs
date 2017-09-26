using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Interfaces.Repositorio
{
    public interface IPerfilRepositorio : IRepositorio<Perfil>
    {
        IEnumerable<Perfil> BuscarPerfisPorCodigo(string[] codigos);
        IEnumerable<Perfil> BuscarPorCodigo(Expression<Func<Perfil, bool>> criterio);
        new Perfil Salvar(Perfil perfil);
        Perfil Atualizar(Perfil objeto);
    }
}