
using ControleAcesso.Dominio.Entidades;
using System.Collections.Generic;

namespace ControleAcesso.Dominio.Interfaces.Repositorio
{

    public interface ISistemaPerfilRepositorio : IRepositorio<SistemaPerfil>
    {
        void AssociarPerfilSistema(List<SistemaPerfil> lstSisPerfil);
        
    }



}
