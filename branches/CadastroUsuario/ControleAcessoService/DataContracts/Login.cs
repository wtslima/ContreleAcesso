
using System;
using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
	[DataContract(Namespace = "http://inmetro.gov.br/ControleAcesso/DataContracts")]
	public class Login
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }
		[DataMember(Order = 2)]
		public string Senha { get; set; }
		
		public override string ToString()
		{
			return string.Format("[Login UserName={0}]", UserName);
		}
	}
}
