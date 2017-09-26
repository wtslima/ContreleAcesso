using System;
using Corporativo.Dominio.Entidades;
using System.Collections.Generic;

namespace ControleAcesso.Dominio.Entidades
{
	public class SistemaPerfil : Controle
	{
		public virtual string CodigoSistema { get; set; }
		public virtual string CodigoPerfil { get; set; }
		
		public virtual Perfil Perfil { get; set; }
		public virtual Sistema Sistema { get; set; }

        public virtual IList<UsuarioExterno> UsuariosExternos { get; set; } 
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (CodigoSistema != null)
					hashCode += 1000000007 * CodigoSistema.GetHashCode();
				if (CodigoPerfil != null)
					hashCode += 1000000009 * CodigoPerfil.GetHashCode();
			}
			return hashCode;
		}
		
		public override string ToString()
		{
			return string.Format("[SistemaPerfil CodigoSistema={0}, CodigoPerfil={1}]", CodigoSistema, CodigoPerfil);
		}
	}
}