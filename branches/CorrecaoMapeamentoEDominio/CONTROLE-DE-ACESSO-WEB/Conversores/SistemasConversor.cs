using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using CONTROLE_DE_ACESSO_WEB.Models.Sistemas;

namespace CONTROLE_DE_ACESSO_WEB.Conversores
{
    public static class SistemasConversor
    {
        public static IEnumerable<SistemasModel> ConverterSistemasModel(this IEnumerable<Sistema> sistemasConverteParametros)
        {
            return sistemasConverteParametros == null ? null : sistemasConverteParametros.Select(s => s.ConverterSistemasModel());
        }

        public static SistemasModel ConverterSistemasModel(this Sistema sistemaConverteParametros)
        {
            var sistema = new SistemasModel()
            {
                Id = sistemaConverteParametros.Id,
                CodSistema = sistemaConverteParametros.Codigo.ToUpper(),
                DescSistema = sistemaConverteParametros.Nome.ToUpper()
            };

            return sistema;
        }

        public static IEnumerable<Sistema> ConverterObjSistemasDominio(this IEnumerable<SistemasModel> paramObjSistemas)
        {
            return paramObjSistemas == null ? null : paramObjSistemas.Select(s => s.ConverterObjSistemasDominio());
        }

        public static Sistema ConverterObjSistemasDominio(this SistemasModel paramObjSistemas)
        {
            var sistema = new Sistema()
            {
                Id = paramObjSistemas.Id,
                Codigo = paramObjSistemas.CodSistema.ToUpper(),
                Nome = paramObjSistemas.DescSistema.ToUpper()
            };

            return sistema;
        }
    }
}