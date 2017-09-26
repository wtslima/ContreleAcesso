using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Entidades;
using Corporativo.Utils.ActiveDirectory;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	public class UsuarioServico : Servico<Usuario>
	{
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
		
		public Usuario AutenticarUsuarioNoActiveDirectory(string login, string senha) {
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
		
		public Usuario AutenticarUsuario(string codigoSistema, string login, string senha) {
			var usuario = AutenticarUsuarioNoActiveDirectory(login, senha);
			var perfisInativos = usuario.Perfis.Where(p => !p.Ativo || !p.SistemaPerfil.Ativo || !p.SistemaPerfil.Perfil.Ativo).ToList();
			perfisInativos.ForEach(p => usuario.RemoverPerfil(p));
			var perfisOutrosSistemas = usuario.Perfis.Where(p => !p.SistemaPerfil.CodigoSistema.ToUpper().Equals(codigoSistema.ToUpper())).ToList();
			perfisOutrosSistemas.ForEach(p => usuario.RemoverPerfil(p));
			
			if (usuario.Perfis.Where(p => p.Ativo).Count() > 0 && usuario.Perfis.Any(p => p.CodigoPerfil.Trim().Equals("AUTENTIC"))) {
				return usuario;
			} else {
				throw new AcessoNegadoException();
			}
		}

		/*public override void Salvar(Usuario usuario)
		{
			Repositorio.SalvarComTransacao(usuario);
		}
		
		public void Excluir(string login) {
			Repositorio.Excluir(u => u.Login == login);
		}*/
	}
}