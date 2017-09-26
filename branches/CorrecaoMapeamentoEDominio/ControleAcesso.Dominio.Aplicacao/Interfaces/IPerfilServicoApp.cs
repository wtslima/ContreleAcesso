using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface IPerfilServicoApp : IServicoApp<Perfil>
    {
        IEnumerable<Perfil> BuscarPerfisPorCodigo(string[] codigo);

        IEnumerable<Perfil> BuscarPorCodigo(Expression<Func<Perfil, bool>> criterio);

        Perfil Cadastrar(Perfil perfil);
        Perfil Atualizar(Perfil perfil);

        void Excluir(int id);
    }
}