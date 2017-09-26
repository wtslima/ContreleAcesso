using System.Collections.Generic;
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
    public class Aplicacao
    {
        ISistemaServicoApp Servico { get; set; }
        private Mock<ISistemaRepositorio> _mock;

        public IEnumerable<EntidadeDominio> SistemaRetorno()
        {
            IList<EntidadeDominio> listaSistema = new List<EntidadeDominio>();

            listaSistema.Add(new EntidadeDominio { Codigo = "Acesso", Nome = "ControleAcesso"});
            listaSistema.Add(new EntidadeDominio { Codigo = "Acesso", Nome = "ControleAcesso" });
            listaSistema.Add(new EntidadeDominio { Codigo = "Acesso", Nome = "ControleAcesso" });
            listaSistema.Add(new EntidadeDominio { Codigo = "Acesso", Nome = "ControleAcesso" });

            return listaSistema;
        }

        public IEnumerable<ObjetoValor> ServidorRetorno()
        {
            IList<ObjetoValor> listaServidor = new List<ObjetoValor>();

            listaServidor.Add(new ObjetoValor ("",1,""));
            listaServidor.Add(new ObjetoValor ("",2,""));
            listaServidor.Add(new ObjetoValor ("",3,""));
            listaServidor.Add(new ObjetoValor ("",4,""));

            return listaServidor;
        }
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<ISistemaRepositorio>();
            Servico = new SistemaServico(_mock.Object);
            SistemaRetorno();
            ServidorRetorno();
        }

        private void InicializarSalvarRepositorio(EntidadeDominio objeto)
        {
            _mock.Setup(s => s.Salvar(objeto));
        }
        [TestCase("ACESSO" ,"Sistema Controle de Acesso", TestName="Cadastrar Sistemas = SUCESSO", Category="")]
        public void CadastrarSistema(string codigo, string nome)
        {
            var sistema = new EntidadeDominio
                          {
                              Codigo = codigo,
                              Nome = nome
                          };
            InicializarSalvarRepositorio(sistema);

            Servico.Salvar(sistema);
        }
    }
}