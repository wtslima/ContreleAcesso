using System;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	/// <summary>
	/// Description of SistemaServico.
	/// </summary>
	public class SistemaServico : Servico<Sistema>
	{
        public static new SistemaServico Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = (SistemaServico)new SistemaServico();
                }

                return (SistemaServico)_instancia;
            }
        }

        public static new SistemaServico NovaInstancia
        {
            get
            {
                _instancia = new SistemaServico();
                return (SistemaServico)_instancia;
            }
        }

		public bool ValidarToken(string token, string userHostName) {

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			var servidor = DecomporToken(token, userHostName);
			if (servidor == null) {
				throw new TokenInvalidoException(token, null);
			}
			
			return servidor != null && !string.IsNullOrWhiteSpace(servidor.CodigoSistema);
		}
		
		public ServidorOrigem DecomporToken(string token, string userHostName) {
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			try {
				var sistema = Buscar(s => s.ServidoresOrigem.Any(serv => serv.Token.Equals(token))).First();
                //TODO: Criptografar as informações oriundas do banco e comparar o token recem-gerado com o token informado na requisição

				return sistema.ServidoresOrigem.FirstOrDefault(serv => serv.Token.Trim().Equals(token.Trim()));
			} catch (Exception ex) {
				throw new TokenInvalidoException(token, ex);
			}
		}
        
        public Sistema BuscarPorToken(string token) {
        	return Buscar(s => s.ServidoresOrigem.Any(ip => ip.Token.Equals(token))).SingleOrDefault();
        }
		
		public void Excluir(string codigoSistema) {
			Repositorio.Excluir(s => s.Codigo.Equals(codigoSistema));
		}
	}
}
