using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using RestSharp;
using NUnit.Framework;
using System.Text;
using ControleAcessoService.DataContracts;


namespace ControleAcesso.Teste.Servicos
{
    public class ServiceRunner
    {
        protected RestClient _cliente;
        protected string _token = ConfigurationManager.AppSettings["token"];
        protected string _credencial = ConfigurationManager.AppSettings["CredencialAcessoAnonimo"];

        protected void Setup(bool autenticar = true)
        {
            _cliente = new RestClient(ConfigurationManager.AppSettings["ControleAcesso.Servico.url"]);
            _cliente.AddDefaultHeader("Content-Type", "application/json");
            _cliente.AddDefaultHeader("Accept", "application/json");

        }

        public List<Usuario> ListarTodosUsuarios()
        {
            Setup();
            var request = new RestRequest("REST/TodosUsuarios?token="+ _token, Method.GET);
            request.RequestFormat = DataFormat.Json;

            var response = _cliente.Execute<List<Usuario>>(request);
            return response.Data;

        }

        public List<Usuario> ObterTodosUsuarios(int filtroTipoUsuario, bool filtroSistema)
        {
            Setup();
            var request = new RestRequest("REST/ObterTodosUsuarios", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(new
                                {
                                    token = _token,
                                    filtro = filtroTipoUsuario,
                                    filtrarPorSistema = filtroSistema

                                });
            var response = _cliente.Execute<List<Usuario>>(request);
            return response.Data;
        }

        public Usuario AutenticarUsuario(string usuario, string senha)
        {
            Setup();
            var request = new RestRequest("REST/AutenticarUsuario", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
                                {
                                    token = _token,
                                    login = new { UserName = usuario, Senha = senha }
                                });
            var response = _cliente.Execute<Usuario>(request);
            return response.Data;

        }

        public Usuario BuscarUsuarioExterno(string userName)
        {
            Setup();
            var request = new RestRequest("REST/BuscarUsuarioExterno", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
                                {
                                    token = _token,
                                    login = userName
                                });
            var response = _cliente.Execute<Usuario>(request);
            return response.Data;
        }

        public Usuario AutenticarUsuarioAnonimo()
        {
            Setup();
            var request = new RestRequest("REST/AutenticarUsuarioAnonimo", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
                                {
                                    token = _token,
                                    credencial = _credencial
                                });
            var response = _cliente.Execute<Usuario>(request);
            return response.Data;
        }

        public Usuario AutenticarUsuarioExterno(string usuario, string senha)
        {
            Setup();
            var request = new RestRequest("REST/AutenticarUsuarioExterno", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
                                {
                                    token = _token,
                                    login = new { UserName = usuario, Senha = senha }
                                });
            var response = _cliente.Execute<Usuario>(request);
            return response.Data;
        }

        public void CriarUsuarioExterno(string login, string nome, string email, string cpf, string codigoSistema, string codigoPerfil, string nomePerfil, int idPessoa, string senha, DateTime nascimento, int tipo)
        {
            Setup();
            var request = new RestRequest("REST/CriarUsuario", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
                                {
                                    token = _token,
                                    usuario = new
                                    {
                                        Login = login,
                                        Nome = nome,
                                        Email = email,
                                        CPF = cpf,
                                        Perfis = new { },
                                        PessoaFisica = new { Id = idPessoa },
                                        Senha = senha,
                                        //Nascimento = nascimento,
                                        //Tipo = tipo

                                    }

                                });
            _cliente.Execute(request);

        }


    }
}
