
using System;
using Corporativo.Infraestrutura.Tipos;

namespace ControleAcesso.Dominio.Infra.Tipos
{
	/// <summary>
	/// Description of AtivacaoTipo.
	/// </summary>
	public class AtivacaoTipo : StringToBoolTipo
	{
		protected override string TrueValue { get { return "A"; } }
		protected override string FalseValue {get { return "D"; } }
	}
}
