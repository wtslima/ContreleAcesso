using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ControleAcesso.Teste.Servicos
{
    [TestFixture]
    public class ServicoTesteAceitacao
    {
        [TestCase]
        public void BuscarTodosUsuario()
        {
            Assert.IsNotNull(new ServicoRunner().ListarTodosUsuarios());

        }
    }
}
