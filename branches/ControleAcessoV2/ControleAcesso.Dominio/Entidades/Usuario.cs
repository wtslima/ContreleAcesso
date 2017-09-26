using Corporativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleAcesso.Dominio.Entidades
{
	public class Usuario : Controle
	{
		private IList<UsuarioSistemaPerfil> _perfis;
		
		public virtual string Login { get; set; }
		public virtual string Nome { get; set; }
		public virtual string Email { get; set; }
        public virtual int IdPessoaFisica { get; set; }
	    public virtual PessoaFisica PessoaFisica { get; set; }
	    /// <summary>
		/// Lista de perfis atribuídos ao usuário.
		/// </summary>
		public virtual IEnumerable<UsuarioSistemaPerfil> Perfis {
            get { return _perfis.Where(p => p.Ativo == true && p.SistemaPerfil.Ativo == true).ToList(); }
		}
		
		public Usuario() {
			_perfis = new List<UsuarioSistemaPerfil>();
		}
		
		public virtual bool AdicionarPerfil(SistemaPerfil perfil) {
            var found = _perfis.FirstOrDefault(p => p.SistemaPerfil.Equals(perfil));
            if (found == null)
            {
                var uesp = new UsuarioSistemaPerfil();
                uesp.CodigoPerfil = perfil.CodigoPerfil;
                uesp.CodigoSistema = perfil.CodigoSistema;
                uesp.LoginUsuario = this.Login;
                uesp.SistemaPerfil = new SistemaPerfil() { CodigoPerfil = perfil.CodigoPerfil, CodigoSistema = perfil.CodigoSistema };
                _perfis.Add(uesp);
                return true;
            }
            else
            {
                found.Ativo = true;
            }

            return false;
		}
		public virtual bool RemoverPerfil(SistemaPerfil perfil) {
            var uesp = _perfis.FirstOrDefault(p => p.SistemaPerfil.CodigoPerfil == perfil.CodigoPerfil && p.CodigoSistema == perfil.CodigoSistema);
            if (uesp != null)
            {
                uesp.Ativo = false;
                uesp.Alteracao = DateTime.Now;
                return true;
            }

            return false;
		}
        public virtual void AtivarDesativarTodosPerfis(string codigoSistema, bool ativo)
        {
            _perfis.Where(p => p.CodigoSistema == codigoSistema).ToList().ForEach(p => p.Ativo = ativo);
        }
		
        public virtual void Contextualizar(string codigoSistema)
        {
            _perfis
                .Where(p => !p.CodigoSistema.Equals(codigoSistema) || p.Ativo == false || p.SistemaPerfil.Ativo == false || p.SistemaPerfil.Perfil.Ativo == false)
                .ToList().ForEach(p => _perfis.Remove(p));
        }

		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			unchecked {
				if (Login != null)
					hashCode += 1000000007 * Login.GetHashCode();
			}
			return hashCode;
		}
		
		public override string ToString()
		{
			return string.Format("[Usuario Login={0}, Nome={1}]", Login, Nome);
		}
	}
}