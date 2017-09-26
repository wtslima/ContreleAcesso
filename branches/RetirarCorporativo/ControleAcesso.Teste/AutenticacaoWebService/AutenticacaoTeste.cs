using System.Linq;
using Castle.Windsor;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using ControleAcesso.Infra.IoC;
using NUnit.Framework;

namespace ControleAcesso.Teste.AutenticacaoWebService
{
    [TestFixture]
    public class AutenticacaoTeste
    {

        private IUsuarioExternoServicoApp _servico;

        public AutenticacaoTeste()
        {
            var container = new WindsorContainer();
            new ConfigurarDependencias().Install(container);
            _servico = container.Resolve<IUsuarioExternoServicoApp>();
        }

        protected string senha ="A9-5B-C1-66-31-AE-2B-6F-AD-B4-55-EE-01-8D-A0-AD-C2-70-3E-56-D8-9E-3E-ED-07-4C-E5-6D-2F-7B-1B-6A";

        [TestCase("SERVIR", "wslima-cast@inmetro.gov.br", "Wellington Silva Lima", TestName = "AUTENTICAR USUÁRIO EXTERNO - SUCESSO", Category = "Autenticacao,WebService")]
        public void AutenticarUsuarioExterno(string codigoSistema, string login, string nome)
        {
            UsuarioExterno usuario = _servico.Autenticar(codigoSistema, login, senha);
            Assert.AreEqual(usuario.Nome, nome);
            Assert.IsNotNull(usuario);
        }
    }
}

   
