using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ControleAcesso.Dominio.ObjetosDeValor;
using NHibernate.Linq;

namespace ControleAcesso.Dominio.Entidades
{
	public class Sistema : Controle
	{
		private HashSet<ServidorOrigem> _servidoresOrigem;
		private HashSet<Perfil> _perfisAcesso;

        [Key]
        public virtual int Id { get; set; }
		public virtual string Codigo { get; set; }
		public virtual string Nome { get; set; }
		public virtual IEnumerable<ServidorOrigem> ServidoresOrigem {
			get { return _servidoresOrigem; }
			protected set { _servidoresOrigem = new HashSet<ServidorOrigem>(value); }
		}
		public virtual IEnumerable<Perfil> PerfisAcesso {
			get { return _perfisAcesso; }
			protected set { _perfisAcesso = new HashSet<Perfil>(value); }
		}
		
		public Sistema() {
			_servidoresOrigem = new HashSet<ServidorOrigem>();
			_perfisAcesso = new HashSet<Perfil>();
		}
		
		public virtual bool AdicionarIpServidorOrigem(string servidorOrigem) {
			var comparer = StringComparer.OrdinalIgnoreCase;
			if (_servidoresOrigem.Any(ip => comparer.Compare(ip.Servidor, servidorOrigem) == 0))
			    return false;
			
			var ipServidor = new ServidorOrigem(Id, _servidoresOrigem.Count + 1, servidorOrigem);
			return _servidoresOrigem.Add(ipServidor);
		}
		
		public virtual bool RemoverIpServidorOrigem(string servidorOrigem) {
			var comparer = StringComparer.OrdinalIgnoreCase;
			var retorno = _servidoresOrigem.RemoveWhere(s => comparer.Compare(s.Servidor, servidorOrigem) == 0) >= 1;
			
			var aux = new HashSet<ServidorOrigem>();
			_servidoresOrigem.ForEach(s => aux.Add(new ServidorOrigem(s.CodigoSistema, aux.Count + 1, s.Servidor)));
			ServidoresOrigem = aux;
			
			return retorno;
		}
		
		public virtual bool AdicionarPerfilAcesso(Perfil perfil) {
			return _perfisAcesso.Add(perfil);
		}
		public virtual bool RemoverPerfilAcesso(Perfil perfil) {
			return _perfisAcesso.Remove(perfil);
		}
		public virtual bool RemoverPerfilAcesso(int codigoPerfil) {
			return _perfisAcesso.RemoveWhere(p => p.Id.Equals(codigoPerfil)) > 0;
		}
		
		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			unchecked {
				if (Codigo != null)
					hashCode += 1000000007 * Codigo.GetHashCode();
			}
			return hashCode;
		}

		public override string ToString()
		{
			return string.Format("[Sistema Nome={0}, Codigo={1}]", Nome, Codigo);
		}
	}
}
