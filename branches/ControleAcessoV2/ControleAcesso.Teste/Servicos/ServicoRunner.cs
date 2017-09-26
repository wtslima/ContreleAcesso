using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using RestSharp;
using System.Linq;
using System.Text;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Teste.Servicos
{
    public class ServicoRunner
    {
        protected RestClient _cliente;
        protected string _token = ConfigurationManager.AppSettings["token"];
 
        protected void Setup(bool autenticar = true)
        {
            _cliente = new RestClient(ConfigurationManager.AppSettings["ControleAcesso.Servico.url"]);
            _cliente.AddDefaultHeader("Content-Type", "application/json");
            _cliente.AddDefaultHeader("Accept", "application/json");

        }

        public List<Usuario> ListarTodosUsuarios()
        {
            Setup();
            var request = new RestRequest("REST/TodosUsuarios?token ="+_token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            var response = _cliente.Execute<List<Usuario>>(request);
            return response.Data;

        }
        
    }
}
