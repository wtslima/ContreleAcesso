using System;
using System.Security.Cryptography;
using System.Text;
using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.ObjetosDeValor
{
	public class ServidorOrigem : IControle
	{
		public virtual int CodigoSistema { get; protected set; }
        public virtual string NomeParametro { get; protected set; }
        public virtual string Servidor { get; protected set; }
        public virtual string Token { get; protected set; }

        public virtual string GerarToken()
        {
			var token = "";
			var chave = CodigoSistema + "|" + Servidor;
			using (var md5 = MD5.Create()) {
				var data = md5.ComputeHash(Encoding.UTF8.GetBytes(chave));
				var strBuilder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					strBuilder.Append(data[i].ToString("x2"));
				}
				
				token = strBuilder.ToString();
			}
			
			return token;
		}
		
		#region IControle
        public virtual bool Excluido { get; set; }
        public virtual DateTime Alteracao { get; set; }
        public virtual string Origem { get; set; }
		#endregion
		
		public ServidorOrigem() {
			NomeParametro = "ServidorOrigem";
			Alteracao = DateTime.Now;
		}
		public ServidorOrigem(int codigoSistema, int indiceParametro, string nomeServidor) {
			CodigoSistema = codigoSistema;
			NomeParametro = string.Format("ServidorOrigem{0}", indiceParametro);
			Servidor = nomeServidor;
			Token = GerarToken();
			Alteracao = DateTime.Now;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (CodigoSistema != null)
					hashCode += 1000000007 * CodigoSistema.GetHashCode();
				if (NomeParametro != null)
					hashCode += 1000000009 * NomeParametro.GetHashCode();
			}
			return hashCode;
		}
		
		public override bool Equals(object obj)
		{
			ServidorOrigem other = obj as ServidorOrigem;
			if (other == null)
				return false;
			return this.GetHashCode() == other.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[ServidorOrigem Servidor={0}, NomeParametro={1}, CodigoSistema={2}]", Servidor, NomeParametro, CodigoSistema);
		}

	}
}