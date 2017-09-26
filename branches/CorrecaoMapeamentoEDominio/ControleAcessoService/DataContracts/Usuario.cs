using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
    public enum TipoUsuario
    {
        Interno = 0,
        Externo = 1
    }

	[Serializable]
	[DataContract(Namespace = "http://inmetro.gov.br/ControleAcesso/DataContracts")]
	public class Usuario
	{
        [DataMember(Order = 1)]
        public int Id { get; set; }
                
        [DataMember(Order = 2)]
		public string Login { get; set; }

        [DataMember(Order = 3)]
		public string Nome { get; set; }
		[DataMember(Order = 4)]
		public string Email { get; set; }
		[DataMember(Order = 5)]
		public string CPF { get; set; }
		[DataMember(Order = 6)]
		public List<Perfil> Perfis { get; set; }

        [DataMember(Order = 7)]
        public PessoaFisica PessoaFisica { get; set; }
		
		#region Propriedades específicas de usuários externos
		[DataMember(Order = 8)]
		public virtual string Senha { get; set; }
		[DataMember(Order = 9)]
		public virtual DateTime Nascimento { get; set; }
		#endregion

        [DataMember(Order = 10)]
        public string ControleAtivo { get; set; }
        [DataMember(Order = 11)]
        public TipoUsuario Tipo { get; set; }
		
		public Usuario() {
			Perfis = new List<Perfil>();
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			Usuario other = obj as Usuario;
			if (other == null)
				return false;
            return this.Id == other.Id &&  this.Login == other.Login && this.Nome == other.Nome && this.Email == other.Email && this.CPF == other.CPF && object.Equals(this.Perfis, other.Perfis);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
                if (Id > 0)
                    hashCode += 1000000007 * Id.GetHashCode();
				if (Login != null)
					hashCode += 1000000007 * Login.GetHashCode();
				if (Nome != null)
					hashCode += 1000000009 * Nome.GetHashCode();
				if (Email != null)
					hashCode += 1000000021 * Email.GetHashCode();
				if (CPF != null)
					hashCode += 1000000033 * CPF.GetHashCode();
				if (Perfis != null)
					hashCode += 1000000087 * Perfis.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(Usuario lhs, Usuario rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(Usuario lhs, Usuario rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
		
		public override string ToString()
		{
			return string.Format("[Usuario Login={0}, Nome={1}]", Login, Nome);
		}
	}
}
