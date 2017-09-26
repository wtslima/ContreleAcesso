using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using Corporativo.Utils.ActiveDirectory;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	public class UsuarioServico : BaseServico<Usuario>, IUsuarioServicoApp
	{
	    private readonly IUsuarioRepositorio _repositorio;
	    public UsuarioServico(IUsuarioRepositorio repositorio) : base(repositorio)
	    {
	        _repositorio = repositorio;
	    }

	    private Usuario AutenticarUsuarioNoActiveDirectory(string login, string senha) {
			var adHelper = new ActiveDirectoryHelper("");
			var user = adHelper.GetUserByLoginName(login);
			if (user != null) {
				var dominio = user.LoginNameWithDomain.Substring(0, user.LoginNameWithDomain.IndexOf(@"\"));
				if (adHelper.ValidateUser(dominio, login, senha)) {
					var lista = _repositorio.Buscar(u => u.Login == login);
					if (lista.Any()) {
						return lista.First();
					}
				    throw new LoginNaoAssociadoAPessoaException(login);
				}
			    throw new SenhaInvalidaException();
			}
		    throw new LoginInexistenteException(login);
		}
		
		public Usuario AutenticarUsuario(string codigoSistema, string login, string senha) {
			var usuario = AutenticarUsuarioNoActiveDirectory(login, senha);
		    usuario.Contextualizar(codigoSistema);
			
			if (usuario.Perfis.Any(p => p.Ativo) && usuario.Perfis.Any(p => p.CodigoPerfil.Trim().Equals("AUTENTIC"))) {
				return usuario;
			}
		    throw new AcessoNegadoException();
		}
	}
}