using System.Collections.Generic;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface ISistemaServicoApp : IServicoApp<Sistema>
    {
        bool ValidarToken(string token);
        ServidorOrigem DecomporToken(string token);
        Sistema BuscarPorToken(string token);
        IEnumerable<Sistema> BuscarTodosSistemasPorCodigoPerfil(string codigoPerfil);
        IEnumerable<Sistema> BuscarPorCodigoOuDescricao(string codigo); 
        void Excluir(int id);
        Sistema Cadastrar(Sistema objeto);
        Sistema Atualizar(Sistema objeto);
        
    }
}