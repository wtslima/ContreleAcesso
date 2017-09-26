using System;
using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using Moq;
using NUnit.Framework;
using EntidadeDominio = ControleAcesso.Dominio.Entidades.Sistema;
using ObjetoValor = ControleAcesso.Dominio.ObjetosDeValor.ServidorOrigem;


namespace ControleAcesso.Teste.Unidade.Sistema
{
    [TestFixture]
    public class SistemaServicoTeste
    {
        ISistemaServicoApp Servico { get; set; }
        private Mock<ISistemaRepositorio> _mock;
        private Mock<IPerfilRepositorio> _mockRepositorio;
        private Mock<IServicoApp<EntidadeDominio>> _mockServicoBase;

        public IEnumerable<EntidadeDominio> SistemaRetorno()
        {
            IList<EntidadeDominio> listaSistema = new List<EntidadeDominio>();

            listaSistema.Add(new EntidadeDominio { Id = 1, Codigo = "Acesso", Nome = "Controle Acesso", Excluido = false });
            listaSistema.Add(new EntidadeDominio { Id = 2, Codigo = "SIGRH", Nome = "RECURSOS HUMANO", Excluido = false });
            listaSistema.Add(new EntidadeDominio { Id = 3, Codigo = "SERVIR", Nome = "AVALIAÇÃO DE ESTABELECIMENTOS", Excluido = false });
            listaSistema.Add(new EntidadeDominio { Id = 4, Codigo = "CORPORATIVO", Nome = "BASE CORPORATIVA", Excluido = false });
            return listaSistema;
        }

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<ISistemaRepositorio>();
            _mockRepositorio = new Mock<IPerfilRepositorio>();
            _mockServicoBase = new Mock<IServicoApp<EntidadeDominio>>();
            Servico = new SistemaServico(_mock.Object);
            SistemaRetorno();
        }

        private void InicializarSalvarRepositorio(EntidadeDominio objeto)
        {
            _mock.Setup(s => s.Cadastrar(objeto)).Returns(objeto);
        }

        private void InicializarExcluirSistema()
        {
            var sistema = SistemaRetorno();
            _mock.Setup(s => s.Excluir(sistema.FirstOrDefault()));
        }

        private void InicializarBuscarSistemas(int id)
        {
            _mockServicoBase.Setup(d => d.Buscar(f => f.Id == id)).Returns(SistemaRetorno().Where(g => g.Id == id));
        }

        [TestCase("XPTO", "Sistema XPTO", TestName = "Deve Cadastrar Sistema", Category = "SistemaServico,Unidade")]
        [TestCase(null, null, TestName = "Não Deve Cadastrar Sistema com objeto NULO", Category = "SistemaServico,Unidade")]
        public void CadastrarSistema(string codigo, string nome)
        {

            var sistema = new EntidadeDominio
            {
                Codigo = codigo,
                Nome = nome
            };
            InicializarSalvarRepositorio(sistema);

            if (sistema.Codigo != null)
            {
                var sistemaCadastrado = Servico.Cadastrar(sistema);
                Assert.AreEqual(sistemaCadastrado.Nome, nome);
            }
            else
            {
                sistema = null;
                Assert.Throws<Exception>(() => Servico.Cadastrar(sistema));
            }

        }
        [TestCase(2, true, TestName = "Deve Excluir Sistema", Category = "SISTEMASERVICO,UNIDADE")]
        [TestCase(10, true, TestName = "Não Deve Excluir Sistema", Category = "SISTEMASERVICO,UNIDADE")]
        public void ExcluirSistema(int id, bool excluido)
        {
            InicializarExcluirSistema();
            InicializarBuscarSistemas(id);

            if (id > 0)
            {
                Servico.Excluir(id);

            }
            else
            {
                Assert.Throws<Exception>(() => Servico.Excluir(id));
            }
        }
        [TestCase(null, TestName = "Deve lançar exceção de token inválido", Category = "SISTEMASERVICO,UNIDADE")]
        public void BuscarServidorOrigemPeloToken(string token)
        {
            Assert.Throws<TokenInvalidoException>(() => Servico.DecomporToken(token));
        }

        public void ValidarToken()
        {


        }

        public void BuscarSistemaPorToken()
        {

        }

        public void BuscarSistemasPorCodigoPerfil()
        {

        }
    }
}
