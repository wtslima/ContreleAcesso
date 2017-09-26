using System.Configuration;
using NUnit.Framework;

namespace ControleAcesso.Teste.AutenticacaoWebService
{
    [TestFixture]
    public class AutorizacaoServicoTeste
    {
        protected string Token = ConfigurationManager.AppSettings["token"];

        //[TestCase("eee2deb1d34bb3973c5799b33421627", TestName = "BuscarTodosUsuarios", Category="Autenticacao,WebService")]
        public void BuscarTodosUsuarios(string token)
        {
            //var servico = new ControleAcessoService.AutenticacaoServico();

            //var usuarios = servico.TodosUsuarios(token);

            //Assert.IsNotEmpty(usuarios);

        }

    }
}