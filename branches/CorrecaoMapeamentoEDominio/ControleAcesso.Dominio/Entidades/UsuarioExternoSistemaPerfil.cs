using System;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioExternoSistemaPerfil : Controle
	{
        public virtual int IdLogin { get; set; }
        public virtual int IdSistema { get; set; }
        public virtual int IdPerfil { get; set; }

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
                if (IdSistema > 0)
                    hashCode += 1000000007 * IdSistema.GetHashCode();
                if (IdPerfil > 0)
                    hashCode += 1000000009 * IdPerfil.GetHashCode();
                if (IdLogin > 0)
                    hashCode += 1000000011 * IdLogin.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[UsuarioExternoSistemaPerfil Usuario={0}, Perfil={1}]", Usuario, SistemaPerfil);
        }
	}
}