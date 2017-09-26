using System.Linq;
using ControleAcesso.Dominio.Entidades;
using Moq;
using NUnit.Framework;
using UserExterno = ControleAcesso.Dominio.Entidades.UsuarioExterno;

namespace ControleAcesso.Teste.Unidade.UsuarioExterno
{
    [TestFixture]
    public class Dominio
    {
        public UserExterno Servico { get; set; }
        private Mock<UserExterno> _mockUsuarioExternoDominio;

        public const string Login = "wtslima@gmail.com";
        public const string Senha = "123mudar";
        public const string codigoSistema = "CONPASS";

        [SetUp]
        public void SetUp()
        {
            _mockUsuarioExternoDominio = new Mock<UserExterno>();
            
            Servico = _mockUsuarioExternoDominio.Object;
            Usuario();
           
        }

        public UserExterno Usuario()
        {
            
            var user = (new UserExterno
            {
                Login = "wtslima@gmail.com",
                Nome = "wellington",
                Ativo = true,
                Email = "wtslima@gmail.com",
            });

            user.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "SERVIR", CodigoPerfil = "AUTENTIC" });
            user.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "SIGRH", CodigoPerfil = "ADMIN" });
            user.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "CORPORATIVO", CodigoPerfil = "ADMIN" });
            user.AdicionarPerfil(new SistemaPerfil { CodigoSistema = "CONPASS", CodigoPerfil = "AUTENTIC" });

            return user;
        }
        
        private void InicializarContextualizar(string codigoSistema)
        {
             _mockUsuarioExternoDominio.Setup(s => s.Contextualizar(codigoSistema));
                
        }

        [TestCase("CONPASS", TestName = "CONTEXTUALIZAR - SUCESSO", Category = "unidade,usuarioExterno")]
        [TestCase("QUALQUER", TestName = "CONTEXTUALIZAR - QUEBRA", Category = "unidade,usuarioExterno")]

        public void Contextualizar(string codigoSistema)
        {
            InicializarContextualizar(codigoSistema);

            var perfil = Usuario();

            perfil.Contextualizar(codigoSistema);

            var resultado = perfil.Perfis.Count();

            Assert.Greater(resultado,0);

        }
    }
}