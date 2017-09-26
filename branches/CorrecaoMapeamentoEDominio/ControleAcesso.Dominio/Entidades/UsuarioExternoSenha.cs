using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControleAcesso.Dominio.Exceptions;

namespace ControleAcesso.Dominio.Entidades
{
    public class UsuarioExternoSenha : Controle
    {
        protected UsuarioExternoSenha() {
            Expiracao = DateTime.MaxValue;
        }
        public UsuarioExternoSenha( string valor, DateTime? expiracao = null, bool isTemporaria = false) : base()
        {
            UsuarioExterno = new UsuarioExterno();// usuarioExterno;
            Valor = valor;
            Expiracao = expiracao ?? DateTime.MaxValue;
            Tipo = isTemporaria ? "T" : "P";
        }

        //public override int Id { get { return UsuarioExterno.Id; } set { UsuarioExterno.Id = value; } }
        public virtual UsuarioExterno UsuarioExterno { get; set; }
        public virtual string Valor { get; set; }
        public virtual DateTime Expiracao { get; set; }
        public virtual string Tipo { get; set; }

        public virtual bool IsTemporaria {
            get {
                return Tipo != null && Tipo.Trim().Equals("T");
            }
        }

        public virtual bool Validar(string senha) {
            if (IsTemporaria) {
                if (DateTime.Now.CompareTo(Expiracao) > 0) {
                    throw new SenhaTemporariaExpiradaException();
                }
            }
            return Valor.Equals(senha);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 100000007 * UsuarioExterno.Login.ToLowerInvariant().GetHashCode();
                hashCode += 100000007 * Tipo.GetHashCode();
                hashCode += 100000007 * Expiracao.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString() {
            if (!IsTemporaria) {
                return UsuarioExterno.Login + " (permanente)";
            } else {
                return UsuarioExterno.Login + " (temporária " + Expiracao.ToString("dd/MM/yyyy hh:mm:ss") + ")";
            }
        }
    }
}