using System.Collections.Generic;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Interfaces
{
    public interface ISistemaPerfilServicoApp : IServicoApp<SistemaPerfil>
    {
        void AssociarPerfilSistema(List<SistemaPerfil> lstSisPerfil);
        

    }
}
