using System;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioSistemaPerfil : Controle
	{
		public virtual int IdLogin { get; set; }
		public virtual int IdSistema { get; set; }
        public virtual int IdPerfil { get; set; }
        public virtual string CodigoAtivo { get; set; }
		
		public virtual Usuario Usuario { get ; set; }
		public virtual SistemaPerfil SistemaPerfil { get; set; }
				
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (IdSistema > 0)
					hashCode += 1000000007 * IdSistema.GetHashCode();
				if (IdPerfil > 0)
					hashCode += 1000000009 * IdPerfil.GetHashCode();
				if (IdLogin != null)
					hashCode += 1000000011 * IdLogin.GetHashCode();
			}
			return hashCode;
		}
		
		public override string ToString()
		{
            return string.Format("[UsuarioSistemaPerfil Usuario={0}, Perfil={1}, CodigoAtivo={2}]", Usuario, SistemaPerfil, CodigoAtivo);
		}
	}
}