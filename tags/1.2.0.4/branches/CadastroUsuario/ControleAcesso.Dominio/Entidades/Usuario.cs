using System;
using System.Collections.Generic;

using Corporativo.Dominio.Entidades;

namespace ControleAcesso.Dominio.Entidades
{
	public class Usuario : Controle
	{
		private HashSet<UsuarioSistemaPerfil> _perfis;
		
		public virtual string Login { get; set; }
		public virtual string Nome { get; set; }
		public virtual string Email { get; set; }
		public virtual string CPF { get; set; }
		/// <summary>
		/// Lista de perfis atribuídos ao usuário.
		/// </summary>
		public virtual IEnumerable<UsuarioSistemaPerfil> Perfis {
			get { return _perfis;}
			set { _perfis = new HashSet<UsuarioSistemaPerfil>(value); }
		}
		
		public Usuario() {
			_perfis = new HashSet<UsuarioSistemaPerfil>();
		}
		
		public virtual bool AdicionarPerfil(UsuarioSistemaPerfil perfil) {
			return _perfis.Add(perfil);
		}
		public virtual bool RemoverPerfil(UsuarioSistemaPerfil perfil) {
			return _perfis.Remove(perfil);
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