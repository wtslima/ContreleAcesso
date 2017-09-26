using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ControleAcesso.Dominio.Aplicacao;
using ControleAcessoService.DataContracts;
using ControleAcessoServico;
using ControleAcessoService;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using NUnit.Framework;
using System.Net.Http;

namespace ControleAcesso.Teste.Servicos
{
    [TestFixture]
    public class TesteAceitacao
    {
        protected string _token = ConfigurationManager.AppSettings["token"];
        private HttpClient _cliente;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ExcluirDadosDeTestes();

            _cliente = new HttpClient();
            _cliente.BaseAddress = new Uri(ConfigurationManager.AppSettings["ControleAcesso.Servico.url"]);
            _cliente.DefaultRequestHeaders.Accept.Clear();
            _cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void ExcluirDadosDeTestes()
        {
            string queryString =
               " DELETE FROM INMETRO.CONTROLEACESSO.TB_LOGIN_EXTERNO_SENHA WHERE CDA_LOGIN_EXTERNO = 'fulano@inmetro.gov.br'";
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["INMETRO"].ConnectionString))
            {
                using (var cmd = new SqlCommand(queryString))
                {
                    try
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
       
        [TestCase(TestName="BuscarTodosUsuarios")]
        public void _001_TodosUsuarios()
        {
            var response = _cliente.GetAsync("Autenticacao/REST/TodosUsuarios?token=" + _token).Result;
            var lista = response.Content.ReadAsAsync<Elemento.UsuarioRetorno.TodosUsuariosRetorno>().Result.TodosUsuariosResult;
            Assert.IsNotNull(lista);
            Assert.IsNotNull(lista.First().CPF);
            Assert.IsNotNull(lista.First().PessoaFisica);
        }

        [TestCase(0, true, TestName = "ObterTodosUsuarios : Todos, verdadeiro")]
        [TestCase(0, false, TestName = "ObterTodosUsuarios : Todos, false")]
        [TestCase(1, true, TestName = "ObterTodosUsuarios : Internos, verdadeiro")]
        [TestCase(1, false, TestName = "ObterTodosUsuarios: Internos, falso" )]
        [TestCase(2, true, TestName = "ObterTodosUsuarios: Externos, verdadeiro")]
        [TestCase(2, false, TestName = "ObterTodosUsuarios: Externos, false")]
        public void _002_ObterTodosUsuarios(int filtroTipoUsuario, bool filtroSistema)
        {

            var filtro = new Elemento.UsuarioRequisicao.FiltroUsuarioRequisicao { token = _token, filtro = filtroTipoUsuario, filtrarPorSistema = filtroSistema};
            var response = _cliente.PostAsJsonAsync<Elemento.UsuarioRequisicao.FiltroUsuarioRequisicao>("Autenticacao/REST/ObterTodosUsuarios", filtro).Result;
            var retorno = response.Content.ReadAsAsync<Elemento.UsuarioRetorno.ObterTodosUsuariosRetorno>().Result.ObterTodosUsuariosResult;
            Assert.IsNotNull(retorno);
            Assert.IsNotNull(retorno.First().CPF);
            Assert.IsNotNull(retorno.First().PessoaFisica);
        }
        

        [TestCase(TestName = "AutenticarUsuarioAnonimo")]
        public void _004_AutenticarUsuarioAnonimo()
        {
            Assert.IsNotNull(new ServiceRunner().AutenticarUsuarioAnonimo());
        }

        [TestCase("flgentil-cast@inmetro.gov.br", "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B", TestName = "AutenticarUsuarioExterno")]
        public void _005_AutenticarUsuarioExterno(string usuario, string senha)
        {
            var auth = new Elemento.UsuarioRequisicao.AutenticacaoRequisicao { token = _token, login = new Login { UserName = usuario, Senha = senha } };
            var response = _cliente.PostAsJsonAsync<Elemento.UsuarioRequisicao.AutenticacaoRequisicao>("Autenticacao/REST/AutenticarUsuarioExterno", auth).Result;
            var retorno = response.Content.ReadAsAsync<Elemento.UsuarioRetorno.AutenticarUsuarioExternoRetorno>().Result.AutenticarUsuarioExternoResult;
            Assert.IsNotNull(retorno);
            Assert.IsNotNull(retorno.CPF);
            Assert.IsNotNull(retorno.PessoaFisica);
        }

        [TestCase("fulano@inmetro.gov.br", "Claudia de Oliveira Borges", "fulano@inmetro.gov.br", "00156575744", 8677, "123mudar","SERVIR", "AUTENTIC" )]
        public void _006_CriarUsuarioexterno(string login, string nome, string email, string cpf, int idPessoa, string senha, string codigoSistema, string codigoPerfil)
        {

           // Assert.Ignore("Verificar UserHostName");
            var usuario = new Elemento.UsuarioRequisicao.NovoUsuarioRequisicao { token = _token, usuario = new Usuario { Login = login, Nome = nome, Email = email, CPF = cpf, Senha = senha, PessoaFisica = new PessoaFisica { Id = idPessoa }, Perfis = new List<Perfil> { new Perfil { CodigoSistema = codigoSistema, CodigoPerfil = codigoPerfil } }, Tipo = TipoUsuario.Externo } };
            var response = _cliente.PostAsJsonAsync<Elemento.UsuarioRequisicao.NovoUsuarioRequisicao>("AutenticacaoServico.svc/REST/CriarUsuario", usuario).Result;
            var usuarioNovo = UsuarioServico.Instancia.Buscar(a => a.PessoaFisica.Id == idPessoa).FirstOrDefault();
            Assert.IsNotNull(usuario);
        }

    }
}
