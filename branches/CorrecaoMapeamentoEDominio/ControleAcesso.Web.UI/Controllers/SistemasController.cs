﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcessoService;
using ControleAcessoService.DataContracts;
using Newtonsoft.Json;
using ControleAcesso.Web.UI.Models;
using App = ControleAcesso.Dominio.Aplicacao;

namespace ControleAcesso.Web.UI.Controllers
{
    public class SistemasController : Controller
    {
        private IServicoApp<Sistema> servico; 
        
        // GET: Sistemas
        public ActionResult Index()
        {
            
            var sistema = servico.Buscar().ToList();

            return View();
        }

       
        
        
        // GET: Sistemas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sistemas/Create
        public ActionResult Create()
        {
            return View();
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
