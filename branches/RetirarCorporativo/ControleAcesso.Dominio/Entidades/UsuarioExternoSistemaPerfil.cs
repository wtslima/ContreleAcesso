using System;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioExternoSistemaPerfil : Controle
	{
        public virtual string LoginUsuario { get; set; }
        public virtual string CodigoSistema { get; set; }
        public virtual string CodigoPerfil { get; set; }

        public virtual UsuarioExterno Usuario { get; set; }
        public virtual SistemaPerfil SistemaPerfil { get; set; }

        public UsuarioExternoSistemaPerfil()
        {
            Usuario = new UsuarioExterno();
            SistemaPerfil = new SistemaPerfil();
        }

        public override bool Equals(IEntidade entidade)
        {
            return Equals((object)entidade);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                if (CodigoSistema != null)
                    hashCode += 1000000007 * CodigoSistema.GetHashCode();
                if (CodigoPerfil != null)
                    hashCode += 1000000009 * CodigoPerfil.GetHashCode();
                if (LoginUsuario != null)
                    hashCode += 1000000011 * LoginUsuario.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[UsuarioExternoSistemaPerfil Usuario={0}, Perfil={1}]", Usuario, SistemaPerfil);
        }
	}
}