using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using Moq;
using NUnit.Framework;
using UserExterno = ControleAcesso.Dominio.Entidades.UsuarioExterno;

namespace ControleAcesso.Teste.Unidade.UsuarioExterno
{
    [TestFixture]
    public class Aplicacao
    {
        public UsuarioExternoServico ServicoUsuarioExterno { get; set; }

        public IUsuarioExternoRepositorio RepositorioUsuarioExterno { get; set; }

        private Mock<IUsuarioExternoRepositorio> _mockRepositorioUsuarioExterno;
        private Mock<IUsuarioExternoRepositorio> _mockUsuarioExternoRepositorio;

        public IRepositorio<UsuarioExternoSenha> RepositorioUsuarioExternoSenha { get; set; }
        private Mock<IRepositorio<UsuarioExternoSenha>> _mockUsuarioExternoSenhaRepositorio;

        public UserExterno Servico { get; set; }
        private Mock<UserExterno> _mockUsuarioExternoDominio;

        public const string Login = "wtslima@gmail.com";
        public const string Senha = "123mudar";
        public const string codigoSistema = "CONPASS";


        [SetUp]
        public void SetUp()
        {

            _mockRepositorioUsuarioExterno = new Mock<IUsuarioExternoRepositorio>();
            _mockUsuarioExternoRepositorio = new Mock<IUsuarioExternoRepositorio>();
            _mockUsuarioExternoSenhaRepositorio = new Mock<IRepositorio<UsuarioExternoSenha>>();

            ServicoUsuarioExterno = new UsuarioExternoServico(_mockRepositorioUsuarioExterno.Object, _mockUsuarioExternoSenhaRepositorio.Object);

            _mockUsuarioExternoDominio = new Mock<UserExterno>();

            Servico = _mockUsuarioExternoDominio.Object;
            RepositorioUsuarioExternoSenha = _mockUsuarioExternoSenhaRepositorio.Object;
            RepositorioUsuarioExterno = _mockUsuarioExternoRepositorio.Object;
            
            UsuarioExternos();
            UsuarioExternoSenhas();
        }

        public IEnumerable<UserExterno> UsuarioExternos()
        {
            IList<UserExterno> lista = new List<UserExterno>();

            lista.Add(new UserExterno
            {
                Login = "wtslima@gmail.com",
                IdPessoaFisica = 123,
                Nome = "wellington",
                Ativo = true,
                Email = "wtslima@gmail.com"
            });

            lista.Add(new UserExterno
            {
                Login = "flavio.gentil@gmail.com",
                IdPessoaFisica = 145,
                Nome = "flavio",
                Ativo = true,
                Email = "flavio.gentil@gmail.com"
            });

            return lista;
        }

        public IEnumerable<UsuarioExternoSenha> UsuarioExternoSenhas()
        {
            IList<UsuarioExternoSenha> listaSenhas = new List<UsuarioExternoSenha>();

            listaSenhas.Add(new UsuarioExternoSenha("wtslima@gmail.com", "123mudar", null, true));
            listaSenhas.Add(new UsuarioExternoSenha("flgentil@gmail.com", "1234mudar", null, true));

            return listaSenhas;
        }

        public IEnumerable<UsuarioExternoSistemaPerfil> UsuarioExternoSistemaPerfils()
        {
            IList<UsuarioExternoSistemaPerfil> listaPerfis = new List<UsuarioExternoSistemaPerfil>();

            listaPerfis.Add(new UsuarioExternoSistemaPerfil { Ativo = true, CodigoPerfil = "AUTENTIC", CodigoSistema = "CONPASS" });

            return listaPerfis;
        }


        private void InicializarCadastrarRepositorio()
        {
            _mockUsuarioExternoRepositorio.Setup(s => s.Cadastrar(It.IsAny<UserExterno>()))
                .Callback<UserExterno>(u =>
                {
                    if (u.Login == "flavio@gmail.com") throw new Exception();
                }
                );
        }

        private void InicializarRepositorioBuscaUsuarioExterno(Expression<Func<UserExterno, bool>> expr)
        {

            _mockRepositorioUsuarioExterno.Setup(x => x.Buscar(expr)).Returns(UsuarioExternos().Where(expr.Compile()));
        }

        public void InicializarValidarSenhaUsuarioExterno()
        {
            _mockUsuarioExternoDominio.Setup(x => x.Autenticar(Senha)).Returns(UsuarioExternoSenhas().FirstOrDefault);

        }

        public void InicializarExcluirUsuarioExternoSenha()
        {
            _mockUsuarioExternoSenhaRepositorio.Setup(s => s.Excluir(UsuarioExternoSenhas().FirstOrDefault()));
        }

        public void InicializarGerarSenhaTemporaria()
        {
            _mockRepositorioUsuarioExterno.Setup(x => x.Salvar(UsuarioExternos().FirstOrDefault()));
        }

        public void InicializarDesativarSenhaPermananente()
        {
            _mockUsuarioExternoSenhaRepositorio.Setup(s => s.Excluir(UsuarioExternoSenhas().FirstOrDefault()));
        }


        [TestCase("wtslima@gmail.com", TestName = "BUSCAR USUARIO EXTERNO - SUCESSO", Category = "unidade,usuarioExterno,")]
        [TestCase("david@gmail.com", TestName = "BUSCAR USUARIO EXTERNO - QUEBRA", Category = "unidade,usuarioExterno,")]
        public void BuscarUsuarioExterno(string login)
        {
            Expression<Func<UserExterno, bool>> expr = u => u.Login.ToLowerInvariant().Trim().Equals(Login.ToLowerInvariant().Trim());
            InicializarRepositorioBuscaUsuarioExterno(expr);
            var resultado = ServicoUsuarioExterno.Buscar(expr);
            var usuario = resultado.First();

            Assert.AreEqual(usuario.Login, login);
        }

        [TestCase("123mudar", "wtslima@gmail.com", TestName = "VALIDAR SENHA - SUCESSO", Category = "unidade,usuarioExterno")]
        [TestCase("1234erro", "wtslima@gmail.com", TestName = "VALIDAR SENHA - QUEBRA", Category = "unidade,usuarioExterno")]
        public void ValidarSenhaUsuarioExterno(string senha, string login)
        {
            InicializarValidarSenhaUsuarioExterno();
            var usuario = Servico.Autenticar(senha);

            Assert.AreEqual(usuario.Login, login);
        }


        [TestCase("wtslima@gmail.com", TestName = "BUSCAR E SALVAR USUARIO EXTERNO - SUCESSO", Category = "unidade,usuarioExterno")]
        [TestCase("flavio@gmail.com", TestName = "BUSCAR E SALVAR USUARIO EXTERNO - QUEBRA", Category = "unidade,usuarioExterno")]
        public void BuscarSalvar(string login)
        {
            Expression<Func<UserExterno, bool>> expr = c => c.Login.ToLowerInvariant().Trim().Equals(Login.ToLowerInvariant().Trim());

            InicializarRepositorioBuscaUsuarioExterno(expr);

            var resultado = ServicoUsuarioExterno.Buscar(expr);
            var usuario = resultado.First();

            Assert.AreEqual(usuario.Login, login);

            ServicoUsuarioExterno.Salvar(usuario);
        }


        [TestCase("wtslima@gmail.com", 123, TestName = "SALVA USÚARIO EXTERNO - SUCESSO", Category = "unidade,usuarioExterno")]
        [TestCase("", 0, TestName = "SALVA USÚARIO EXTERNO - QUEBRA (exceção não tratada)", Category = "unidade,usuarioExterno")]
        [TestCase("flavio.gentil@gmail.com", 100, TestName = "SALVA USÚARIO EXTERNO - QUEBRA (login já existe)", Category = "unidade,usuarioExterno")]
        [TestCase("flavio.gentil@gmail.com", 145, TestName = "SALVA USÚARIO EXTERNO - QUEBRA (pessoa física já cadastrada)", Category = "unidade,usuarioExterno")]
        public void SalvarUsuarioExterno(string login, int idPessoa)
        {
            _mockUsuarioExternoRepositorio.Setup(s => s.Cadastrar(It.IsAny<UserExterno>()))
               .Callback<UserExterno>(u =>
               {
                   if (u.Login == "flavio.gentil@gmail.com" || string.IsNullOrWhiteSpace(u.Login)) throw new Exception();
               }
               );
            _mockUsuarioExternoRepositorio.Setup(s => s.Buscar(It.IsAny<Expression<Func<UserExterno, bool>>>()))
                .Returns<Expression<Func<UserExterno, bool>>>(exp => UsuarioExternos().Where(exp.Compile()));
            ServicoUsuarioExterno = new UsuarioExternoServico(_mockUsuarioExternoRepositorio.Object, _mockUsuarioExternoSenhaRepositorio.Object);
            var usuario = new UserExterno{ Login = login, IdPessoaFisica = idPessoa };
            try
            {
                ServicoUsuarioExterno.Cadastrar(usuario);
            }
            catch (Exception ex)
            {
                if (idPessoa == 145) Assert.That(ex, Is.InstanceOf<UsuarioCadastradoException>());
                if (idPessoa == 100) Assert.That(ex, Is.InstanceOf<LoginCadastradoException>());
            }
        }


        [TestCase("wtslima@gmail.com", "teste@nova", TestName = "ALTERAR SENHA USUÁRIO EXTERNO - SUCESSO", Category = "unidade, usuarioExterno")]
        [TestCase("flavio@gmail.com", "123@345r", TestName = "ALTERAR SENHA USUÁRIO EXTERNO - QUEBRA", Category = "unidade, usuarioExterno")]
        public void AlterarSenhausuarioExterno(string login, string senhaNova)
        {
            InicializarExcluirUsuarioExternoSenha();
            
            var usuario = UsuarioExternos().FirstOrDefault();
            
            usuario.AdicionarSenha(UsuarioExternoSenhas().FirstOrDefault());
            
            if (usuario.SenhaTemporaria.IsTemporaria);
            {
                RepositorioUsuarioExternoSenha.Excluir(usuario.SenhaTemporaria);

                usuario.AdicionarSenha(new UsuarioExternoSenha(login, senhaNova, null, false));
            }

            Assert.AreEqual(usuario.Senhas,senhaNova);

            ServicoUsuarioExterno.Salvar(usuario);

        }

        [TestCase("wtslima@gmail.com", "1234@teste",  TestName = "GERAR SENHA TEMPORÁRIA - SUCESSO", Category = "unidade, usuarioExterno")]
        [TestCase("flavio@gmail.com", "teste@teste", TestName = "GERAR SENHA TEMPORÁRIA - QUEBRA", Category = "unidade, usuarioExterno")]
        public void GerarSenhaTemporariaUsuarioExterno(string login, string senhaNova)
        {
            Expression<Func<UserExterno, bool>> expr = u => u.Login.ToUpperInvariant().Equals(login.ToUpperInvariant().Trim());

            InicializarRepositorioBuscaUsuarioExterno(expr);
            InicializarGerarSenhaTemporaria();

            var resultado = ServicoUsuarioExterno.Buscar(expr);
            var usuario = resultado.First();
            
            usuario.AdicionarSenha(new UsuarioExternoSenha(login, senhaNova, null, true));

            RepositorioUsuarioExterno.Salvar(usuario);

        }

        [TestCase("wtslima@gmail.com", "1234@teste", TestName = "GERAR SENHA TEMPORÁRIA - SUCESSO", Category = "unidade, usuarioExterno")]
        [TestCase("flavio@gmail.com", "teste@teste", TestName = "GERAR SENHA TEMPORÁRIA - QUEBRA", Category = "unidade, usuarioExterno")]
        public void DesativarSenhaPermanenteUsuarioExterno(string login, string senhaNova)
        {
            Expression<Func<UserExterno, bool>> expr = p => p.Login.Trim().ToLower().Equals(Login.Trim().ToLower());

            InicializarRepositorioBuscaUsuarioExterno(expr);
            InicializarDesativarSenhaPermananente();

            var resultado = RepositorioUsuarioExterno.Buscar(expr);
            var usuario = resultado.First();

            RepositorioUsuarioExternoSenha.Excluir(usuario.Senha);

        }
    }
}
