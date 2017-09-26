using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioExterno : Controle
	{
        private int _senhaIndex = -1, _senhaTemporariaIndex = -1;
        private List<UsuarioExternoSenha> _senhas;
        private HashSet<SistemaPerfil> _perfis;

		public virtual string Login { get; set; }
		public virtual string Email { get; set; }
        public virtual int IdPessoaFisica { get; set; }
        public virtual string Nome { get; set; }
        public virtual IEnumerable<UsuarioExternoSenha> Senhas {
            get {
                return _senhas.ToArray();
            }
            protected set {
                _senhas = new List<UsuarioExternoSenha>(value);

                for (var i = 0; i < _senhas.Count(); i++) {
                    if (!_senhas[i].IsTemporaria)
                        _senhaIndex = i;
                    else
                        _senhaTemporariaIndex = i;
                }
            }
        }
        public virtual UsuarioExternoSenha Senha {
            get {
                return _senhaIndex >= 0 ? _senhas[_senhaIndex] : null;
            }
        }
        public virtual UsuarioExternoSenha SenhaTemporaria {
            get {
                return _senhaTemporariaIndex >= 0 ? _senhas[_senhaTemporariaIndex] : null;
            }
        }
		
		/// <summary>
		/// Lista de perfis atribuídos ao usuário.
		/// </summary>
        public virtual IEnumerable<SistemaPerfil> Perfis
        {
			get { return _perfis;}
            set { _perfis = new HashSet<SistemaPerfil>(value); }
		}
		
		public UsuarioExterno() {
            _perfis = new HashSet<SistemaPerfil>();
            _senhas = new List<UsuarioExternoSenha>();
		}

        public virtual bool AdicionarPerfil(SistemaPerfil perfil)
        {
			return _perfis.Add(perfil);
		}
        public virtual bool RemoverPerfil(SistemaPerfil perfil)
        {
			return _perfis.Remove(perfil);
		}

        public virtual void AdicionarSenha(UsuarioExternoSenha senha)
        {
            senha.UsuarioExterno = this;

            _senhas.Add(senha);
            if (senha.IsTemporaria)
            {
                _senhaTemporariaIndex = _senhas.IndexOf(senha);
            }
            else
            {
                _senhaIndex = _senhas.IndexOf(senha);
            }
        }

        public virtual UsuarioExternoSenha Autenticar(string senha)
        {
            if (SenhaTemporaria != null && SenhaTemporaria.Validar(senha))
                return SenhaTemporaria;

            if (SenhaTemporaria == null)
            {
                if (Senha != null && Senha.Validar(senha))
                    return Senha;
            }

            return null;
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
			return string.Format("[Usuario Login={0}, Nome={1}]", Login, Login);
		}
	}
}
