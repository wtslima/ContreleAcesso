using System;
using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    public class SistemaPerfilServico : BaseServico<SistemaPerfil>, ISistemaPerfilServicoApp
    {

        private readonly ISistemaPerfilRepositorio _repositorio;
        private readonly ISistemaServicoApp _sistemaservico;
        private readonly IPerfilServicoApp _perfilservico;

        public SistemaPerfilServico(ISistemaPerfilRepositorio repositorio, ISistemaServicoApp sistemaservico, IPerfilServicoApp perfilservico) : base(repositorio)
        {
            _repositorio = repositorio;
            _sistemaservico = sistemaservico;
            _perfilservico = perfilservico;
        }
        
        public void AssociarPerfilSistema(List<SistemaPerfil> lstSisPerfil)
        {
            if (lstSisPerfil.Count > 0)
            {
                var _perfil =_perfilservico.Buscar(s => s.Id == lstSisPerfil[0].CodigoPerfil).FirstOrDefault();

                if (_perfil == null)
                {
                    throw new Exception();
                }

                foreach(var sistemaperfil in lstSisPerfil)
                {
                    var _sistema = _sistemaservico.Buscar(s => s.Id == sistemaperfil.CodigoSistema).FirstOrDefault();

                    if (_sistema == null)
                    {
                        throw new Exception();
                    }

                    sistemaperfil.Origem = "I";
                }

                _repositorio.AssociarPerfilSistema(lstSisPerfil);
                
            }
            else
            {
                throw new Exception();
            }




        }
        

    }
}
