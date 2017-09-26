using System.Configuration;
using NUnit.Framework;

namespace ControleAcesso.Teste.AutenticacaoServico
{
    [TestFixture]
    public class AutorizacaoServicoTeste
    {
        protected string _token = ConfigurationManager.AppSettings["token"];

        [TestCase("eee2deb1d34bb3973c5799b33421627", TestName = "BuscarTodosUsuarios")]
        public void BuscarTodosUsuarios(string token)
        {
            var servico = new ControleAcessoService.AutenticacaoServico();

            var usuarios = servico.TodosUsuarios(token);

            Assert.IsNotEmpty(usuarios);

        }

    }
}