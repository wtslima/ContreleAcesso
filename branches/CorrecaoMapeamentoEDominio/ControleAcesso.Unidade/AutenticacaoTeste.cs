using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Configuration;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Integracao
{
    [TestFixture]
    public class AutenticacaoTeste
    {
        protected string senha = "6E-5B-A6-DB-ED-8F-27-29-4C-A4-35-04-B3-7D-01-19-B6-3F-17-EA-CF-25-4C-43-2F-89-EE-79-82-0A-99-5B";
        [TestCase("SERVIR","flgentil-cast@inmetro.gov.br", "Flávio Luis Pereira Gentil", TestName="AUTENTICAR USUARIO EXTERNO - SUCESSO", Category="Autenticar-Integracao")]
        public void AutenticarUsuarioExterno(string sistema, string login, string nome)
        {
            var usuario = UsuarioExternoServico.NovaInstancia.Autenticar(sistema, login, nome); ;

            Assert.AreEqual(usuario.Nome, nome);
            Assert.IsNotNull(usuario);

        }
    }
}
