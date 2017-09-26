using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using NUnit.Framework;
using Servico = ControleAcesso.Dominio.Aplicacao.Servicos.UsuarioExternoServico;

namespace ControleAcesso.Teste.Aplicacao
{
    [TestFixture]
    public class UsuarioExternoServico
    {
        private readonly IUsuarioExternoRepositorio _repositorioUsuario;
        private readonly IRepositorio<UsuarioExternoSenha> _repositorioUsuarioSenha;
       
        [TestCase(TestName="Autenticar Injeção")]
        public void AutenticarUsuarioExterno()
        {
            Servico _servico = new Servico(_repositorioUsuario, _repositorioUsuarioSenha);
            var resultado = _servico.Autenticar(2 , "dcbelmont@inmetro.gov.br",
                "49-CA-65-CB-FE-EE-35-84-D1-C2-B0-4F-CF-D5-71-87-C7-AE-23-A3-65-85-D3-56-1D-22-8C-D4-DE-68-65-41");

            Assert.NotNull(resultado);


        }
    }
}