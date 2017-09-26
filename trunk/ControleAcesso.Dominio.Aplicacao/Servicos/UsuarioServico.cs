using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Infra.Repositorios;
using Corporativo.Utils.ActiveDirectory;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	public class UsuarioServico : Servico<Usuario>
	{
        public UsuarioServico() : base()
        {
            Repositorio = new UsuarioRepositorio();
        }

	    public static new UsuarioServico Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = (UsuarioServico)new UsuarioServico();
                }

                return (UsuarioServico)_instancia;
            }
        }

        public static new UsuarioServico NovaInstancia
        {
            get
            {
                _instancia = new UsuarioServico();
                return (UsuarioServico)_instancia;
            }
        }
		
		public virtual Usuario AutenticarUsuarioNoActiveDirectory(string login, string senha) {
			ActiveDirectoryHelper adHelper = new ActiveDirectoryHelper("");
			var user = adHelper.GetUserByLoginName(login);
			if (user != null) {
				var dominio = user.LoginNameWithDomain.Substring(0, user.LoginNameWithDomain.IndexOf(@"\"));
				if (adHelper.ValidateUser(dominio, login, senha)) {
					var lista = Buscar(u => u.Login == login);
					if (lista.Count() > 0) {
						return lista.First();
					} else {
						throw new LoginNaoAssociadoAPessoaException(login);
					}
				} else {
					throw new SenhaInvalidaException();
				}
			} else {
				throw new LoginInexistenteException(login);
			}
		}
		
		public virtual Usuario AutenticarUsuario(string codigoSistema, string login, string senha) {
			var usuario = AutenticarUsuarioNoActiveDirectory(login, senha);
		    usuario.Contextualizar(codigoSistema);
			
			if (usuario.Perfis.Any(p => p.Ativo) && usuario.Perfis.Any(p => p.CodigoPerfil.Trim().Equals("AUTENTIC"))) {
				return usuario;
			}
		    throw new AcessoNegadoException();
		}
	}
}