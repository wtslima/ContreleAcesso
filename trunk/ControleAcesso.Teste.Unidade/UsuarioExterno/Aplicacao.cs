using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using Corporativo.Dominio.Repositorios;
using Moq;
using NHibernate.Mapping.ByCode.Impl;
using NUnit.Framework;
using UserExterno = ControleAcesso.Dominio.Entidades.UsuarioExterno;

namespace ControleAcesso.Teste.Unidade.UsuarioExterno
{
    [TestFixture]
    public class Aplicacao
    {
        public UsuarioExternoServico ServicoUsuarioExterno { get; set; }
        private Mock<UsuarioExternoServico> _MockServicoUsuarioExterno;

        public IRepositorio<UserExterno> ServicoApp { get; set; }
        private Mock<IRepositorio<UserExterno>> _mockUsuarioRepositorio;
        
        public UserExterno Servico { get; set; }
        private Mock<UserExterno> _mockUsuarioExternoDominio;
        
        public const string Login = "wtslima@gmail.com";
        public const string Senha = "123mudar";
        public const string codigoSistema = "CONPASS";
       

        [SetUp]
        public void SetUp()
        {

            _MockServicoUsuarioExterno = new Mock<UsuarioExternoServico>();
            ServicoUsuarioExterno = _MockServicoUsuarioExterno.Object;
            InicializarServicoUsuarioExterno();

            _mockUsuarioRepositorio = new Mock<IRepositorio<UserExterno>>();
            _mockUsuarioExternoDominio = new Mock<UserExterno>();
            Servico = _mockUsuarioExternoDominio.Object;
            ServicoApp = _mockUsuarioRepositorio.Object;
            UsuarioExternos();
            UsuarioExternoSenhas();
            InicializarRepositorioBuscaUsuarioExterno();
            InicializarValidarSenhaUsuarioExterno();
        }

        public IEnumerable<UserExterno> UsuarioExternos()
        {
            IList<UserExterno> lista = new List<UserExterno>();

            lista.Add(new UserExterno
            {
                Login = "wtslima@gmail.com", 
                Nome = "wellington", 
                Ativo = true,
                Email = "wtslima@gmail.com",
                
              
            });


            return lista;
        }

        public IEnumerable<UsuarioExternoSenha> UsuarioExternoSenhas()
        {
            IList<UsuarioExternoSenha> listaSenhas = new List<UsuarioExternoSenha>();

            listaSenhas.Add(new UsuarioExternoSenha("wtslima@gmail.com", "123mudar", null, false));
            listaSenhas.Add(new UsuarioExternoSenha("flgentil@gmail.com", "1234mudar", null, true));

            return listaSenhas;
        }

        public IEnumerable<UsuarioExternoSistemaPerfil> UsuarioExternoSistemaPerfils()
        {
            IList<UsuarioExternoSistemaPerfil>  listaPerfis = new List<UsuarioExternoSistemaPerfil>();

            listaPerfis.Add(new UsuarioExternoSistemaPerfil{Ativo = true, CodigoPerfil = "AUTENTIC", CodigoSistema = "CONPASS"});

            return listaPerfis;
        }

        private void InicializarSalvarRepositorio()
        {
            _mockUsuarioRepositorio.Setup(s => s.Salvar(UsuarioExternos().FirstOrDefault()));
        }
        
        private void InicializarRepositorioBuscaUsuarioExterno()
        {
           
            _mockUsuarioRepositorio.Setup(
                x => x.Buscar(u => u.Login.ToLowerInvariant().Trim().Equals(Login.ToLowerInvariant().Trim())))
                .Returns(UsuarioExternos().Where(c => c.Login == Login));
        }

        

        public void InicializarValidarSenhaUsuarioExterno()
        {
            _mockUsuarioExternoDominio.Setup(x => x.Autenticar(Senha)).Returns(UsuarioExternoSenhas().FirstOrDefault);
            
        }

        [TestCase("wellington", TestName = "Buscar USUARIO EXTERNO - SUCESSO", Category = "usuarioExterno,")]
        public void BuscarUsuarioExterno(string nome)
        {
            InicializarRepositorioBuscaUsuarioExterno();
            var resultado = ServicoApp.Buscar(c => c.Login.ToLowerInvariant().Trim().Equals(Login.ToLowerInvariant().Trim()));
            var usuario = resultado.First();

           
            Assert.AreEqual(usuario.Nome, nome);
        }

        [TestCase("123mudar", "wtslima@gmail.com", TestName = "Validar Senha - Sucesso", Category = "usuarioExterno,unidade")]
        public void ValidarSenhaUsuarioExterno(string senha, string login)
        {
            InicializarValidarSenhaUsuarioExterno();
            var usuario = Servico.Autenticar(senha);

            Assert.AreEqual(usuario.Login, login);
        }

        private void InicializarServicoUsuarioExterno()
        {
            _MockServicoUsuarioExterno.Setup(x => x.Autenticar("CONPASS", Login, Senha)).Returns(UsuarioExternos().FirstOrDefault());
        }

        [TestCase("CONPASS", "123mudar", "wtslima@gmail.com", TestName = "Validar Usuario Externo - Sucesso", Category = "usuarioExterno,unidade")]
        public void ValidarUsuarioExterno(string codigoSistema, string senha, string login)
        {

            var resultado = ServicoUsuarioExterno.Autenticar(codigoSistema, login, senha);

            Assert.AreEqual(resultado.Login, login);
        }

        //[TestCase("wtslima@gmail.com", TestName = "Buscar e Salva Usuário Externo - SUCESSO", Category = "usuarioExterno,unidade")]
        //public void BuscarSalvar(string login)
        //{
        //    InicializarRepositorioBuscaUsuarioExterno();
        //    var resultado = ServicoApp.Buscar(c => c.Login.ToLowerInvariant().Trim().Equals(Login.ToLowerInvariant().Trim()));
        //    var usuario = resultado.First();

        //    ServicoApp.Salvar(usuario);
            
        //}
        [TestCase(TestName = "Buscar e Salva Usuário Externo - SUCESSO", Category = "usuarioExterno,unidade")]
        public void SalvarUsuarioExterno()
        {
            InicializarSalvarRepositorio();

            ServicoApp.Salvar(UsuarioExternos().FirstOrDefault());

        }
    }
}
