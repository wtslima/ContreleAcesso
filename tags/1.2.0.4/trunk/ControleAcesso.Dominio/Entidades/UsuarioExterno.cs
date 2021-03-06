﻿using Corporativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleAcesso.Dominio.Entidades
{
	public class UsuarioExterno : Controle
	{
        private int _senhaIndex = -1, _senhaTemporariaIndex = -1;
        private List<UsuarioExternoSenha> _senhas;
        private IList<UsuarioExternoSistemaPerfil> _perfis;

		public virtual string Login { get; set; }
		public virtual string Email { get; set; }
        public virtual int IdPessoaFisica { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
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
        public virtual IEnumerable<UsuarioExternoSistemaPerfil> Perfis
        {
            get { return _perfis.Where(p => p.Ativo == true).ToList(); }
		}
		
		public UsuarioExterno() {
            _perfis = new List<UsuarioExternoSistemaPerfil>();
            _senhas = new List<UsuarioExternoSenha>();
		}

        public virtual bool AdicionarPerfil(SistemaPerfil perfil)
        {
            var found = _perfis.FirstOrDefault(p => p.SistemaPerfil != null &&
                                                    p.SistemaPerfil.Equals(perfil));
            if (found == null)
            {
                var uesp = new UsuarioExternoSistemaPerfil();
                uesp.CodigoPerfil = perfil.CodigoPerfil;
                uesp.CodigoSistema = perfil.CodigoSistema;
                uesp.SistemaPerfil = new SistemaPerfil() { CodigoPerfil = perfil.CodigoPerfil, CodigoSistema=perfil.CodigoSistema };
                uesp.LoginUsuario = this.Login;
                _perfis.Add(uesp);
                return true;
            }
            else
            {
                found.Ativo = true;
            }

            return false;
        }
        public virtual bool RemoverPerfil(SistemaPerfil perfil)
        {
            var uesp = _perfis.FirstOrDefault(p => p.SistemaPerfil.Equals(perfil));
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
