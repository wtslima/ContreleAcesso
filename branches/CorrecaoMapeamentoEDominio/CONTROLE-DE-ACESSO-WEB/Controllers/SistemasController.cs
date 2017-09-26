using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using CONTROLE_DE_ACESSO_WEB.Models.Sistemas;
using CONTROLE_DE_ACESSO_WEB.Conversores;
using PagedList;

namespace CONTROLE_DE_ACESSO_WEB.Controllers
{
    public class SistemasController : BaseController<Sistema>
    {
        public new ISistemaServicoApp ServicoAplicacao { get; set; }

        public SistemasController(ISistemaServicoApp servicoAplicacao) : base(servicoAplicacao)
        {
        }

        // GET: Sistemas
        public ActionResult Index(string sistema, int? pagina)
        {
            if (!string.IsNullOrWhiteSpace(sistema) || TempData["pesqInicial"] != null)
            {
                var sistemaPesquisado = !string.IsNullOrWhiteSpace(sistema) ? sistema : TempData["pesqInicial"].ToString();
                var sistemas = ServicoAplicacao.BuscarPorCodigoOuDescricao(sistemaPesquisado);
                var sistemaModel = sistemas.Select(c => c.ConverterSistemasModel());
                ViewBag.Paginacao = sistemaModel.OrderBy(x => x.CodSistema).ToList().ToPagedList(pagina ?? 1, 15);
            }

            return View();
        }

        // GET: Buscar Sistemas pelo Código

        [Authorize]
        [HttpGet]
        public ActionResult ListarSistemasPorCodigo(SistemasModel parametros, int? pagina)
        {
            TempData["pesqInicial"] = parametros.CodSistema;

            var sistemas = ServicoAplicacao.BuscarPorCodigoOuDescricao(parametros.CodSistema);

            var sistemaModel = sistemas.Select(c => c.ConverterSistemasModel());

            var paginaTamanho = 15;
            var paginaNumero = (pagina ?? 1);

            return PartialView("ListarAsyncPartial", sistemaModel.OrderBy(x => x.CodSistema).ToList().ToPagedList(paginaNumero, paginaTamanho));
        }

        // POST: Cadastrar sistemas
        [Authorize]
        [HttpPost]
        public ActionResult CadastrarSistemas(SistemasModel parametros)
        {
            if (parametros == null) ;
            SistemasModel convertidoParaModel = null;
            try
            {
                var cadSistemaModel = parametros.ConverterObjSistemasDominio();

                var sistemas = ServicoAplicacao.Cadastrar(cadSistemaModel);

                convertidoParaModel = sistemas.ConverterSistemasModel();

                return PartialView("ModalsPartial", convertidoParaModel);

            }
            catch (Exception)
            {
                Exception ex;
            }
            return PartialView("ModalsPartial", convertidoParaModel);
            ;
        }

        // POST: Editar Sistemas
        [Authorize]
        [HttpPost]
        public ActionResult EditarSistemas(SistemasModel parametros)
        {
            if (parametros == null) ;
            SistemasModel convertidoParaModel = null;
            try
            {
                var cadSistemaModel = parametros.ConverterObjSistemasDominio();

                var sistemas = ServicoAplicacao.Atualizar(cadSistemaModel);

                convertidoParaModel = sistemas.ConverterSistemasModel();

                // Validando se está vazio

                if (TempData["pesqInicial"] != null)
                {
                    //Necessário TypeCasting para tipos complexos.
                    TempData.Keep("pesqInicial");

                    return PartialView("ModalsPartial", TempData.Values);
                }
                return PartialView("ModalsPartial", convertidoParaModel);
            }
            catch (Exception)
            {
                Exception ex;
            }

            {
                if (TempData["pesqInicial"] != null)
                {
                    TempData.Keep("pesqInicial");

                    return PartialView("ModalsPartial", TempData.Values);
                }
                return PartialView("ModalsPartial", convertidoParaModel);
            }
        }


// POST: Sistemas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sistemas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sistemas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sistemas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sistemas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
