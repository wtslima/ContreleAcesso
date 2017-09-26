using Corporativo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ControleAcesso.Dominio.Entidades
{
    public class Usuario : Controle
    {
        private IList<UsuarioSistemaPerfil> _perfis;

        [Key]
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual int IdPessoaFisica { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        /// <summary>
        /// Lista de perfis atribuídos ao usuário.
        /// </summary>
        public virtual IEnumerable<UsuarioSistemaPerfil> Perfis
        {
            get { return _perfis.Where(p => p.Excluido == false && p.SistemaPerfil.Excluido == false).ToList(); }
        }

        public Usuario()
        {
            _perfis = new List<UsuarioSistemaPerfil>();
        }

        public virtual bool AdicionarPerfil(SistemaPerfil perfil)
        {
            var found = _perfis.FirstOrDefault(p => p.SistemaPerfil.Equals(perfil));
            if (found == null)
            {
                var uesp = new UsuarioSistemaPerfil();
                uesp.IdPerfil = perfil.CodigoPerfil;
                uesp.IdSistema = perfil.CodigoSistema;
                uesp.IdLogin = Id;
                uesp.SistemaPerfil = new SistemaPerfil() { CodigoPerfil = perfil.CodigoPerfil, CodigoSistema = perfil.CodigoSistema };
                _perfis.Add(uesp);
                return true;
            }
            found.Excluido = false;

            return false;
        }
        public virtual bool RemoverPerfil(SistemaPerfil perfil)
        {
            var uesp = _perfis.FirstOrDefault(p => p.SistemaPerfil.CodigoPerfil == perfil.CodigoPerfil && p.IdSistema == perfil.CodigoSistema);
            if (uesp != null)
            {
                uesp.Excluido = true;
                uesp.Alteracao = DateTime.Now;
                return true;
            }

            return false;
        }
        public virtual void AtivarDesativarTodosPerfis(int codigoSistema, bool excluido)
        {
            _perfis.Where(p => p.IdSistema == codigoSistema).ToList().ForEach(p => p.Excluido = excluido);
        }

        public virtual void Contextualizar(int codigoSistema)
        {
            _perfis
                .Where(p => !p.IdSistema.Equals(codigoSistema) || p.Excluido == true || p.SistemaPerfil.Excluido == true || p.SistemaPerfil.Perfil.Excluido == true)
                .ToList().ForEach(p => _perfis.Remove(p));
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            unchecked
            { 
                if ( Id > 0 )
                    hashCode += 1000000007 * Id.GetHashCode();

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