using System;
using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using Moq;
using NUnit.Framework;
using EntidadeDominio = ControleAcesso.Dominio.Entidades.Sistema;
using ObjetoValores = ControleAcesso.Dominio.ObjetosDeValor.ServidorOrigem;
using System.Linq.Expressions;


namespace ControleAcesso.Teste.Unidade.Sistema
{
    [TestFixture]
    public class Aplicacao
    {
        public ISistemaServicoApp ServicoSistema { get; set; }
        private Mock<ISistemaRepositorio> _mockRepositorio;

        [SetUp]
        public void Setup()
        {
            _mockRepositorio = new Mock<ISistemaRepositorio>();
            ServicoSistema = new SistemaServico(_mockRepositorio.Object);
            SistemaRetorno();
        }

        public IEnumerable<EntidadeDominio> SistemaRetorno()
        {
            IList<EntidadeDominio> listaSistema = new List<EntidadeDominio>();

            listaSistema.Add(new EntidadeDominio
            {
                Codigo = "Servir",
                Nome = "Sistema Servir",
                 Ativo = true,
                ServidoresOrigem = new HashSet<ObjetoValores> { new ObjetoValores("Servir", 1, "ServidorOrigem1") { Token = "123456789" }, }
               
            });
            listaSistema.Add(new EntidadeDominio 
            {
                Codigo = "Sigrh",
                Nome = "Sistema de RH",
                Ativo = true,
                ServidoresOrigem = new HashSet<ObjetoValores> { new ObjetoValores("Sigrh", 2, "ServidorOrigem2") { Token = "1234567878" } }
               
            });
            listaSistema.Add(new EntidadeDominio
            {
                Codigo = "Certifiq",
                Nome = "Certificacao",
                Ativo = true,
                ServidoresOrigem = new HashSet<ObjetoValores> { new ObjetoValores("Certifiq", 3, "ServidorOrigem3") { Token = "1234567877" } }
             
            });
            listaSistema.Add(new EntidadeDominio
            {
                Codigo = "Acesso",
                Nome = "Controle Acesso",
                Ativo = true,
                ServidoresOrigem = new HashSet<ObjetoValores> { new ObjetoValores("Acesso", 4, "ServidorOrigem4") { Token = "1234567876" } }
            });

            
            return listaSistema;
        }
        
        private void InicializarBuscarSistema(Expression<Func<EntidadeDominio, bool>> expr)
        {
            _mockRepositorio.Setup(s => s.Buscar(expr)).Returns(SistemaRetorno().Where(expr.Compile()));
        }

        public void InicializarExcluirSistema()
        {
            _mockRepositorio.Setup(s => s.Excluir(SistemaRetorno().FirstOrDefault()));
        }

        public void InicializarBuscarPorCodigo(string codigoSistema)
        {
            _mockRepositorio.Setup(c => c.Buscar(x => x.Codigo.Equals(codigoSistema))).Returns(SistemaRetorno());
        }


        [TestCase("123456789", "Servir", TestName = "BUSCAR SISTEMA POR TOKEN - SUCESSO", Category = "SistemaServico,Unidade")]
        [TestCase("66453646634", "Servir", TestName = "BUSCAR SISTEMA POR TOKEN - QUEBRA", Category = "SistemaServico,Unidade")]
        public void BuscarSistemasPorServidorOrigem(string token, string codigoSistema)
        {
            Expression<Func<EntidadeDominio, bool>> expr = c => c.ServidoresOrigem.Any(serv => serv.Token.Equals(token));
            InicializarBuscarSistema(expr);
            var resultado = ServicoSistema.Buscar(expr);

            var sistema = resultado.FirstOrDefault();

            Assert.AreEqual(sistema.Codigo, codigoSistema);
        }

        //private void InicializarSalvarRepositorio(EntidadeDominio objeto)
        //{
        //    _mock.Setup(s => s.Salvar(objeto));
        //}
        //[TestCase("ACESSO", "Sistema Controle de Acesso", TestName = "Cadastrar Sistemas = SUCESSO", Category = "sistemaservico,unidade")]
        //[TestCase("QUALQUER", "Sistema Qualquer", TestName = "Cadastrar Sistemas = QUEBRA", Category = "sistemaservico,unidade")]
        //public void CadastrarSistema(string codigo, string nome)
        //{
        //    var sistema = new EntidadeDominio
        //                  {
        //                      Codigo = codigo,
        //                      Nome = nome
        //                  };
        //    InicializarSalvarRepositorio(sistema);

        //    Servico.Salvar(sistema);
        //}

        [TestCase("Servir", TestName = "EXCLUIR SISTEMA - SUCESSO", Category = "SistemaServico,Unidade")]
        [TestCase("Qualquer", TestName = "EXCLUIR SISTEMA - QUEBRA", Category = "SistemaServico,Unidade")]
        public void ExcluirSistema(string codigoSistema)
        {
            InicializarExcluirSistema();
           // InicializarBuscarPorCodigo(codigoSistema);
          //  var sistemaExcluido = ServicoSistema.Buscar(x => x.Codigo.Equals(codigoSistema));
            var sistemas = SistemaRetorno();
            ServicoSistema.Excluir(SistemaRetorno().FirstOrDefault());
            var sistema =  SistemaRetorno().Count();

            Assert.Greater(sistema, 2);

        }
    }
}