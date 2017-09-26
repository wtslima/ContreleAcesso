using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
	[Serializable]
	[DataContract(Namespace = "http://inmetro.gov.br/ControleAcesso/DataContracts")]
	public class Perfil
	{
		[DataMember(Order = 1)]
		public string CodigoSistema { get; set; }
		[DataMember(Order = 2)]
		public string CodigoPerfil { get; set; }
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			Perfil other = obj as Perfil;
			if (other == null)
				return false;
			return this.CodigoSistema == other.CodigoSistema && this.CodigoPerfil == other.CodigoPerfil;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (CodigoPerfil != null)
					hashCode += 1000000009 * CodigoPerfil.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(Perfil lhs, Perfil rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(Perfil lhs, Perfil rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
		
		public override string ToString()
		{
			return string.Format("[Perfil CodigoSistema={0}, CodigoPerfil={1}]", CodigoSistema, CodigoPerfil);
		}
	}
}
