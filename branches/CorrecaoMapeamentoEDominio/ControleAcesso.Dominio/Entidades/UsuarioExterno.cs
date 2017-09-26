using System.Security.Cryptography;
using System.Text;
using Corporativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioExterno : Controle
	{
        private int _senhaIndex = -1, _senhaTemporariaIndex = -1;
        private IList<UsuarioExternoSenha> _senhas;
        private IList<UsuarioExternoSistemaPerfil> _perfis;

        [Key]
        public virtual int Id { get; set; }
		public virtual string Login { get; set; }
		public virtual string Email { get; set; }
        public virtual int IdPessoaFisica { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual string Nome { get; set; }
        public virtual IEnumerable<UsuarioExternoSenha> Senhas {
            get
            {
                return _senhas.ToArray();
            }
        }
        public virtual UsuarioExternoSenha Senha {
            get{
                return _senhas.FirstOrDefault(x => x.Excluido == false);
            }
        }
		
		/// <summary>
		/// Lista de perfis atribuídos ao usuário.
		/// </summary>
        public virtual IEnumerable<UsuarioExternoSistemaPerfil> Perfis
        {
            get { return _perfis.Where(p => p.Excluido == false && p.SistemaPerfil.Excluido == false).ToList(); }
		}
        
	    public UsuarioExterno() {
            _perfis = new List<UsuarioExternoSistemaPerfil>();
            _senhas = new List<UsuarioExternoSenha>();
		}


        public virtual bool RemoverPerfil(SistemaPerfil perfil)
        {
            var uesp = _perfis.FirstOrDefault(p => p.SistemaPerfil.Equals(perfil));
            if (uesp != null)
            {
                uesp.Excluido = true;
                uesp.Alteracao = DateTime.Now;
                return true;
            }

            return false;
        }
        public virtual void AtivarDesativarTodosPerfis(int IdSistema, bool excluido)
        {
            _perfis.Where(p => p.IdSistema == IdSistema).ToList().ForEach(p => p.Excluido = excluido);
        }

        public virtual void Contextualizar(int IdSistema)
        {
            _perfis
                .Where(p => !p.IdSistema.Equals(IdSistema))
                .ToList().ForEach(p => _perfis.Remove(p));
        }

        public virtual bool AdicionarPerfil(SistemaPerfil perfil)
        {
            var found = _perfis.FirstOrDefault(p => p.SistemaPerfil != null &&
                                                    p.SistemaPerfil.Equals(perfil));
            if (found == null)
            {
                var uesp = new UsuarioExternoSistemaPerfil();
                uesp.IdPerfil = perfil.CodigoPerfil;
                uesp.IdSistema = perfil.CodigoSistema;
                uesp.SistemaPerfil = new SistemaPerfil() { CodigoPerfil = perfil.CodigoPerfil, CodigoSistema = perfil.CodigoSistema };
                uesp.IdLogin = Id;
                uesp.Usuario = this;
                _perfis.Add(uesp);
                return true;
            }

            found.Excluido = false;

            return false;
        }

        public virtual void AdicionarSenha(string valorSenha, DateTime? expiracao = null, bool ehTemporaria = false)
        {
            var senhaCriptografada = Criptografar(valorSenha);
            Senhas.ToList().ForEach(s => s.Excluido = true);
            var senha = new UsuarioExternoSenha(senhaCriptografada, expiracao, ehTemporaria);
            senha.UsuarioExterno = this;
            _senhas.Add(senha);
        }

        public virtual UsuarioExternoSenha Autenticar(string senha)
        {
            if (Senha != null && Senha.Valor.Equals(Criptografar(senha)))
                return Senha;
            
            return  null;
        }

	    public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			unchecked {

                if (Id > 0)
                    hashCode += 1000000007 * Id.GetHashCode();

				if (Login != null)
					hashCode += 1000000007 * Login.ToUpperInvariant().GetHashCode();
			}
			return hashCode;
		}

        private static string Criptografar(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var value = new SHA256Managed().ComputeHash(bytes);
            return BitConverter.ToString(value);
        } 
		
		public override string ToString()
		{
			return string.Format("[UsuarioExterno Login={0}, Nome={1}]", Login, Nome);
		}
	}
}